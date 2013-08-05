<?php

//é utf-8

/**
 * @package Services Test Console
 * @subpackage Web Service Information
 * @author Ricardo Sismeiro <ricardosismeiro@yahoo.co.uk>
 * @version 1.0
 *
 */
class FakeSoapClient extends SoapClient
{

  function __construct($wsdl, $options)
  {
    parent::__construct($wsdl, $options);
  }

  function __doRequest($request, $location, $action, $version, $oneWay = 1)
  {
    //nothing to do, return a empty Envelope
    $result = '<?xml version="1.0" encoding="UTF-8"?>' . PHP_EOL;
    $result .= '<!-- action="' . $action . '" -->' . PHP_EOL;
    $result .= '<!-- location="' . $location . '" -->' . PHP_EOL;
    $result .= '<SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/">' . PHP_EOL;
    $result .= '<SOAP-ENV:Header/>' . PHP_EOL;
    $result .= '<SOAP-ENV:Body/>' . PHP_EOL;
    $result .= '</SOAP-ENV:Envelope>' . PHP_EOL;
    return $result;
  }

}

class DevSoapClient extends SoapClient
{

  function __construct($wsdl, $options)
  {
    parent::__construct($wsdl, $options);
  }

  /**
   * redirect request to another server ...
   */
  function __doRequest($request, $location, $action, $version, $oneWay = 1)
  {
    //$location = strtr($location, array('://production.server/service' => '://development.server/service'));
    //$action =  strtr($action, array('://production.server/service' => '://development.server/service'));
    $result = parent::__doRequest($request, $location, $action, $version, $oneWay);
    return $result;
  }

}

class WSInfo
{

  public $_fakeClient;
  private $_methods;
  private $_parameters;
  public $_client;
  public $result;

  const DEVMODE = true;

  public function __construct()
  {

  }

  public static function objectToArray($obj)
  {
    $result = $obj;
    if (is_object($obj)) {
      $result = (array) $obj;
      foreach ($result as $k => $v) {
        if (is_object($v)) {
          $result[$k] = self::objectToArray($v);
        }
      }
    }
    return $result;
  }

  public function getMethodsList()
  {
    return $this->_methods;
  }

  private function _getParameterFor($method)
  {
    return isset($this->_parameters[$method]) ? $this->_parameters[$method] : null;
  }

  public function createSoapClient($wsdl, $header = null)
  {
    try {
      $client = new SoapClient($wsdl);
      if (!is_object($client) || (!$client instanceof SoapClient)) {
        throw new Exception('client is not a SoapClient');
      }
    } catch (Exception $e) {
      echo 'Error: service is unreachable, try again later!';
      exit;
    }

    $types = $client->__getTypes();
    $m = array();
    if (preg_match_all('/(\w+)\((\w+) /', implode(' ', $client->__getFunctions()), $m)) {
      $this->_methods = $m[1];
      foreach ($this->_methods as $k => $method) {
        if (isset($m[2][$k])) {
          $this->_parameters[$method] = $m[2][$k];
        }
      }
      unset($m);
    }

    unset($client);
    $classmap = array();

    foreach ($types as $struct) {
      if (strpos($struct, 'struct ') === 0) {
        $matches = array();
        if (preg_match('/struct (\w+) \{(.+?)\}/', str_replace("\n", '/n', str_replace("\r", '', $struct)), $matches)) {
          $class = $matches[1];
          $prop = '';
          $matches2 = array();
          if (preg_match_all('/(\w+);/', $matches[2], $matches2)) {
            $prop = $matches2[1];
          }
          if (!empty($prop) && !empty($class)) {
            $code = ' class ' . $class . ' { ';
            foreach ($prop as $p) {
              if (strlen($p) > 0) {
                $code .= 'public $' . $p . '; ';
              }
            }
            $code .='}';
            if (!class_exists($class)) {
              eval($code);
            }
            $classmap[$class] = $class;
          }
        }
      }
    }
    try {
      $this->_fakeClient = new FakeSoapClient($wsdl, array('classmap' => $classmap, 'trace' => true));
      if (self::DEVMODE) {
        $this->_client = new DevSoapClient($wsdl, array('classmap' => $classmap, 'trace' => true));
      } else {
        $origErrorReporting = error_reporting();
        error_reporting(0);
        ini_set('default_socket_timeout', 1);
        $this->_client = new SoapClient($wsdl, array('classmap' => $classmap, 'trace' => true, 'connection_timeout' => 25));
        error_reporting($origErrorReporting);
      }

      if (!is_object($this->_client) || (!$this->_client instanceof SoapClient)) {
        throw new Exception('_client is not a SoapClient');
      }
    } catch (SoapFault $exception) {
      echo 'Error: service is unreachable, try again later!', "\n";
      echo $exception->faultstring;
      exit;
    } catch (Exception $exception) {
      echo 'Error: service is unreachable, try again later!', "\n";
      echo $exception->getMessage();
      exit;
    }


    if (is_array($header) && !empty($header)) {
      $toAdd = array();

      foreach ($header as $head) {
        if (!is_array($head)) {
          $head = array($head);
        }

        if (!empty($head['name']) && class_exists($head['name'])) {
          $headers = null;
          $object = new $head['name'];

          if (isset($head['data']) && is_array($head['data'])) {
            foreach ($head['data'] as $p => $value) {
              if (property_exists($head['name'], $p)) {
                $object->$p = $value;
              }
            }
          }

          if (!empty($head['namespace'])) {
            $headers = new SoapHeader($head['namespace'], $head['name'], $object);
            $toAdd[] = $headers;
          }
        } elseif (!empty($head['name']) && !empty($head['namespace']) && isset($head['data'])) {
          $headers = new SoapHeader($head['namespace'], $head['name'], $head['data']);
          $toAdd[] = $headers;
        }
      }

      $this->_fakeClient->__setSoapHeaders($toAdd);
      $this->_client->__setSoapHeaders($toAdd);
    }
  }

  private function _getSoapCall($client, $method, $request)
  {
    $result = array('parameters' => null, 'result' => null, 'error' => null, 'responseSOAP' => null, 'requestSOAP' => null);
    $parameterClassName = $this->_getParameterFor($method);

    $parameters = new stdClass;

    if (!is_null($parameterClassName) && class_exists($parameterClassName)) {
      unset($parameters);
      $parameters = new $parameterClassName;
      $definedVars = get_class_vars($parameterClassName);
      $vars = array();
      foreach ($request as $key => $value) {
        if (array_key_exists($key, $definedVars)) {
          $vars[$key] = $value;
        }
      }
      foreach ($vars as $key => $value) {
        $className = ucfirst($key);
        if (class_exists($className) && is_array($value)) {
          try {
            foreach ($value as $k => $v) {
              if (is_null($parameters->$key)) {
                $parameters->$key = new $className;
              }
              if (property_exists($parameters->$key, $k)) {
                if (!is_array($v)) {
                  if ('false' == $v) {
                    $v = 0;
                  }
                  $parameters->$key->$k = $v;
                } else {
                  $parameters->$key->$k = array();
                  foreach ($v as $k2 => $v2) {
                    $_tmp = null;
                    if (is_numeric($k2)) {
                      if (is_array($v2)) {
                        list($k3) = array_keys($v2);
                        $classNamek3 = ucfirst(strtolower($k3));
                        if (class_exists($classNamek3) && is_array($v2[$k3])) {
                          $_tmp = new $classNamek3;
                          foreach ($v2[$k3] as $k4 => $v4) {
                            if (property_exists($_tmp, $k4)) {
                              if (!is_array($v4)) {
                                if ('false' == $v4) {
                                  $v4 = 0;
                                }
                                $_tmp->$k4 = $v4;
                              } else {
                                throw new Exception('ERROR : ' . __LINE__ . ' FILE: ' . basename(__FILE__), '1');
                              }
                            }
                          }
                        }
                      }
                    } else {

                      if (is_array($parameters->$key->$k) && empty($parameters->$key->$k)) {
                        $className2 = ucfirst($k);
                        if (class_exists($className2) && is_array($v)) {
                          $parameters->$key->$k = new $className2;
                        }
                      }

                      if (is_object($parameters->$key->$k)) {
                        if (property_exists($parameters->$key->$k, $k2)) {
                          $parameters->$key->$k->$k2 = $v2;
                        }
                      } else {
                        throw new Exception('ERROR : ' . __LINE__ . ' FILE: ' . basename(__FILE__), '1');
                      }
                    }
                    if (!is_null($_tmp)) {
                      array_push($parameters->$key->$k, $_tmp);
                    }
                  }
                }
              }
            }
          } catch (Exception $e) {
            echo $e->getMessage();
            exit;
          }
        } else {
          try {
            if (!is_array($value)) {
              if ('false' == $value) {
                $value = 0;
              }
              $parameters->$key = $value;
            } else {
              $parameters->$key = array();
              foreach ($value as $_k => $_v) {
                if (is_numeric($_k) && !is_array($_v)) {
                  if ('false' == $_v) {
                    $_v = 0;
                  }
                  array_push($parameters->$key, $_v);
                } else {
                  throw new Exception('ERROR: ' . __LINE__ . ' FILE: ' . basename(__FILE__), '1');
                }
              }
            }
          } catch (Exception $e) {
            echo $e->getMessage();
            exit;
          }
        }
      }
    }

    $result['parameters'] = $parameters;
    $endTime = 0;
    try {

      $origErrorReporting = error_reporting();
      error_reporting(0);
      $iniTime = microtime(true);
      $result['result'] = $client->$method($parameters);
      $endTime = round(microtime(true) - $iniTime, 3);
      error_reporting($origErrorReporting);
    } catch (SoapFault $e) {
      //$result['error'] = $e->faultstring;
    } catch (Exception $e) {
      $result['error'] = $e->getMessage();
    }

    $result['responseSOAP'] = $client->__getLastResponse();
    $result['requestSOAP'] = $client->__getLastRequest();
    $result['requestTime'] = $endTime;

    $this->result = $result;
    return $result;
  }

  function getSoapRequest($method, $request)
  {
    $result = null;
    $r = $this->_getSoapCall($this->_fakeClient, $method, $request);
    $h = $this->_fakeClient->__getLastRequestHeaders();
    if (is_null($r['error'])) {
      $result = $h . PHP_EOL . str_replace('><', ">\n<", $r['requestSOAP']);
    } else {
      $result = $r['error'];
    }
    return $result;
  }

  function getSoapAction($method, $request)
  {
    $result = '';
    $r = $this->_getSoapCall($this->_fakeClient, $method, $request);
    if (is_null($r['error'])) {
      $r = isset($r['responseSOAP']) ? $r['responseSOAP'] : '';
      $match = array();
      if (preg_match('/<\!\-\- action="(.+?)" \-\->/', $r, $match)) {
        $result = $match[1];
      }
    }
    return $result;
  }

  function getSoapLocation($method, $request)
  {
    $result = '';
    $r = $this->_getSoapCall($this->_fakeClient, $method, $request);
    if (is_null($r['error'])) {
      $r = isset($r['responseSOAP']) ? $r['responseSOAP'] : '';
      $match = array();
      if (preg_match('/<\!\-\- location="(.+?)" \-\->/', $r, $match)) {
        $result = $match[1];
      }
    }
    return $result;
  }

  function getSoapResponse($method, $request)
  {
    $result = null;

    $r = $this->_getSoapCall($this->_client, $method, $request);
    $h = $this->_client->__getLastRequestHeaders();
    $h .= $this->_client->__getLastRequest();
    $h .= "<!-- RESPONSE -->";
    $h .= $this->_client->__getLastResponseHeaders();
    if (is_null($r['error'])) {
      $result = $h . str_replace('><', ">\n<", $r['responseSOAP']);
      if (self::DEVMODE) {
        $result = str_replace('<SOAP-ENV:Envelope', PHP_EOL . '<!-- ' . $this->result['requestTime'] . ' sec -->' . PHP_EOL . '<SOAP-ENV:Envelope', $result);
      }
    } else {
      $result = $r['error'];
    }
    return $result;
  }

  function getSoapResult($method, $request)
  {
    $result = null;
    if (isset($this->result['result'])) {
      $result = $this->result['result'];
    } else {
      $r = $this->_getSoapCall($this->_client, $method, $request);
      if (is_null($r['error'])) {
        $result = $r['result'];
      } else {
        $result = $r['error'];
      }
    }
    return $result;
  }

  function getJSONResult($method, $request)
  {
    if (isset($this->result['result'])) {
      $array = self::objectToArray($this->result['result']);
    } else {
      $r = $this->_getSoapCall($this->_client, $method, $request);
      if (is_null($r['error'])) {
        $array = self::objectToArray($r['result']);
      } else {
        $array = self::objectToArray($r['error']);
      }
    }
    return json_encode($array);
  }

  function getXMLResult($method, $request)
  {
    $result = null;
    $error = true;

    if (isset($this->result['result'])) {
      $result = $this->result['result'];
    } else {
      $r = $this->_getSoapCall($this->_client, $method, $request);
      if (null === $r['error']) {
        $result = $r['result'];
      } else {
        $result = '<error>' . $r['error'] . '</error>';
      }
    }

    try {
      if (!is_null($result)) {
        $array = self::objectToArray($result);
        if (1 == count($array)) {
          list($root) = array_keys($array);
          $xml = new WSXml();
          $result = $xml->array2xml(WSXml::format($array[$root]), $root);
          $error = false;
        }
      }
    } catch (Exception $e) {
        // do something with $e->getMessage();      
    }

    if ($error) {
      $result = '<' . '?xml version="1.0" encoding="utf8"?' . '>' . "\n";
    }
    $result = preg_replace('/(<\/.+?>)/', '\1' . "\n", $result);
    return $result;
  }

}