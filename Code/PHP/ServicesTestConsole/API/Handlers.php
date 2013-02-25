<?php

//é utf-8

/**
 * @package Services Test Console
 * @author Ricardo Sismeiro <ricardosismeiro@yahoo.co.uk>
 * @version 1.0
 *
 */
interface Handler
{

  function dispatchRequest();
}

class DefineHandler implements Handler
{

  public function dispatchRequest()
  {
    FDML::loadSourceFrom(_CONFIG_DIRECTORY_ . '/' . _CONFIG_SERVICENAME_ . '.php');
    $definitions = FDML::getDefinitions();
    $list = isset($_REQUEST['list']) ? preg_replace('/[^a-zA-Z0-9\-_]/', '', $_REQUEST['list']) : '';

    if ('methods' == $list) {
      $resultTmp = array_keys($definitions);
      $result = array('result' => array('ok' => true, 'methods' => $resultTmp));
    } else {

      $method = isset($_REQUEST['method']) ? preg_replace('/[^a-zA-Z0-9\-_]/', '', $_REQUEST['method']) : '';
      if (!isset($definitions[$method])) {
        $result = array('result' => array('ok' => false));
        return $result;
      }
      $result = array('result' => array('ok' => true, 'input' => array()));
      foreach ($definitions[$method]['definitionsProcessed'] as $definition) {
        if (FDML::isInput($definition)) {
          $inputDefinition = FDML::genInputField($definition);
          foreach ($inputDefinition as $inputMetadata) {
            $result['result']['input'][] = $inputMetadata;
          }
        }
      }
    }

    header('Content-Type: application/json; chatset=UTF-8', true, 200);
    echo json_encode($result);
  }

}

class BuildHandler implements Handler
{

  public function getWSDL()
  {
    $result = str_replace('\\', '/', _CONFIG_DIRECTORY_ . '/' . _CONFIG_SERVICENAME_ . '.wsdl');
    return $result;
  }

  public function getRequest()
  {
    $request = array_merge($_GET, $_POST);
    $exceptions = array('action', 'method', 'ESBAccessKey', 'ESBToken', 'ESBUsername', 'ESBPassword', 'json', 'xml', 'soap');
    foreach ($exceptions as $value) {
      if (isset($request[$value]))
        unset($request[$value]);
    }
    return $request;
  }

  private function _getCredentials()
  {
    $username = isset($_REQUEST['ESBUsername']) ? $_REQUEST['ESBUsername'] : '';
    $password = isset($_REQUEST['ESBPassword']) ? $_REQUEST['ESBPassword'] : '';
    $token = isset($_REQUEST['ESBToken']) ? $_REQUEST['ESBToken'] : '';

    if (isset($_REQUEST['ESBToken'])) {
      $data['ESBToken'] = $token;
    } else {
      $data['ESBUsername'] = $username;
      $data['ESBPassword'] = $password;
    }

    $result = array(
      'name' => 'ESBCredentials',
      'namespace' => 'http://services.sapo.pt/definitions',
      'data' => $data
    );

    return $result;
  }

  private function _getAccessKey()
  {
    $accessKey = isset($_REQUEST['ESBAccessKey']) ? $_REQUEST['ESBAccessKey'] : '';

    $result = array(
      'name' => 'ESBAccessKey',
      'namespace' => 'http://services.sapo.pt/definitions/Market',
      'data' => $accessKey
    );

    return $result;
  }

  public function getHeaders()
  {
    return array($this->_getCredentials(), $this->_getAccessKey());
  }

  public function dispatchRequest()
  {
    $soap = isset($_REQUEST['soap']) ? true : false;

    $method = isset($_REQUEST['method']) ? $_REQUEST['method'] : '';

    $wsdl = $this->getWSDL();
    $request = $this->getRequest();
    $headers = $this->getHeaders();

    $console = new WSInfo;
    $console->createSoapClient($wsdl, $headers);

    //SOAP
    $result = '';
    if ($soap) {
      header('Content-Type: text/xml; charset=UTF-8', true, 200);
      $result = $console->getSoapRequest($method, $request);
    }

    echo $result;
  }

}

class CallHandler extends BuildHandler
{

  public function dispatchRequest()
  {
    $method = isset($_REQUEST['method']) ? $_REQUEST['method'] : '';
    $json = isset($_REQUEST['json']) ? true : false;
    $xml = isset($_REQUEST['xml']) ? true : false;

    $wsdl = $this->getWSDL();
    $request = $this->getRequest();
    $headers = $this->getHeaders();

    $console = new WSInfo;
    $console->createSoapClient($wsdl, $headers);

    if ($json) {
      //JSON
      header('Content-Type: application/json; chatset=UTF-8', true, 200);
      $result = $console->getJSONResult($method, $request);
    } elseif ($xml) {
      //XML
      header('Content-Type: text/xml; charset=UTF-8', true, 200);
      $result = $console->getXMLResult($method, $request);
    } else {
      //SOAP
      header('Content-Type: text/xml; charset=UTF-8', true, 200);
      $result = $console->getSoapResponse($method, $request);
    }
    echo $result;
  }

}