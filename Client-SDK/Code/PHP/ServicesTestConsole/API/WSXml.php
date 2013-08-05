<?php

//é utf-8

/**
 * @package Services Test Console
 * @subpackage Web Service Extensible Markup Language
 * @author Ricardo Sismeiro <ricardosismeiro@yahoo.co.uk>
 * @version 1.0
 *
 */
class WSXml
{

  private $doc;
  private $filename;
  public $result;
  public $rootname;

  function __construct()
  {
    $this->result = false;
  }

  public function load_file($filename)
  {
    if (is_readable($filename)) {
      $this->filename = $filename;
      $this->doc = simplexml_load_file($filename);
    }
  }

  public function load_string($string)
  {
    if ($string != '') {
      $this->doc = simplexml_load_string($string);
    }
  }

  private function convert_simplexml_object2array(&$result, $root, $rootname = 'root')
  {
    $n = count($root->children());
    if ($n > 0) {
      if (!isset($result[$rootname]['@attributes'])) {
        $result[$rootname]['@attributes'] = array();
        foreach ($root->attributes() as $atr => $value)
          $result[$rootname]['@attributes'][$atr] = (string) $value;
      }

      foreach ($root->children() as $child) {
        $name = $child->getName();
        $this->{__FUNCTION__}($result[$rootname][], $child, $name);
      }
    } else {
      $result[$rootname] = (array) $root;
      if (!isset($result[$rootname]['@attributes']))
        $result[$rootname]['@attributes'] = array();
    }
  }

  private function convert_array2simplexml_object($array, $doc = '')
  {
    if (is_array($array)) {
      if (!is_object($doc))
        $doc = $this->doc;

      if ((isset($array['@attributes'])) && (count($array['@attributes']) > 0)) {
        foreach ($array['@attributes'] as $attribute => $value)
          $doc->addAttribute($attribute, utf8_encode($value));
        unset($array['@attributes']);
      }

      foreach ($array as $key => $value) {
        if (is_numeric($key) && is_array($value)) {
          list($child) = array_keys($value);
          if (is_array($value[$child][0])) {
            $newchild = $doc->addChild($child);
          } else {
            $newchild = $doc->addChild($child, utf8_encode($value[$child][0]));
          }
          $this->{__FUNCTION__}($value[$child], $newchild);
        }
      }
    }
  }

  public function xml2array($save_result = false)
  {
    $result = false;
    if (is_object($this->doc)) {
      $result = array();
      $this->rootname = $this->doc->getName();
      $this->convert_simplexml_object2array($result, $this->doc, $this->rootname);
      (isset($result[$this->rootname])) ? ($result = $result[$this->rootname]) : ($result = false);
    }
    if ($save_result)
      $this->result = $result;
    return $result;
  }

  public function array2xml($array, $rootname, $save_result = false)
  {
    $xml_string = '<?xml version=\'1.0\' encoding=\'utf8\'?' . '>' . PHP_EOL;
    $xml_string.='<' . $rootname . '>' . PHP_EOL;
    $xml_string.='</' . $rootname . '>';
    $this->load_string($xml_string);
    $this->convert_array2simplexml_object($array);
    $result = $this->doc->asXML();
    if ($save_result)
      $this->result = $result;
    return $result;
  }

  public static function format($array)
  {
    $result = array();

    foreach ($array as $k => $v) {
      if (!is_numeric($k)) {
        if (is_array($v)) {
          $v = self::format($v);
        } else {
          if (is_bool($v)) {
            $v = $v ? 'true' : 'false';
          }
          $v = array($v);
        }
        $result[] = array($k => $v);
      }
    }
    return $result;
  }

}