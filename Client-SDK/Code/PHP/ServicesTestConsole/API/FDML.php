<?php

//é utf-8

/**
 * @package Services Test Console
 * @subpackage File Definition Markup Language
 * @author Ricardo Sismeiro <ricardosismeiro@yahoo.co.uk>
 * @version 1.0
 *
 */
class FDML
{

  const FDML_TAG = '@describe';
  const FDML_NEW_LINE = '__FDMLNL__';
  const FDML_WHITE_SPACE = '__FDMLWS__';
  const FDML_RESERVED_WORDS = 'type,name,label,limit,allow';
  const FDML_RANGE_WORDS = 'text,textarea,checkbox,radio,password,json,xml,raw,get,post,soap,secureget,securepost,securesoap';
  const FDML_RULES_WORDS = 'input,output,method';

  public static $souce;

  public function __construct()
  {

  }

  public static function isInput($array)
  {
    return (is_array($array) && 'input' == $array[0]) ? true : false;
  }

  public static function loadSourceFrom($filename)
  {
    $result = false;
    if (file_exists($filename) && is_readable($filename)) {
      self::$souce = file_get_contents($filename);
      $result = true;
    }
    return $result;
  }

  public static function getDefinitions()
  {
    $raw = self::getRaw(self::$souce);
    return self::validateLexico($raw);
  }

  public static function isRow($value)
  {
    return (false !== strpos($value, self::FDML_TAG)) ? true : false;
  }

  public static function isRowNotEmpty($value)
  {
    return !empty($value);
  }

  public static function getRowContent($value)
  {
    $result = explode(self::FDML_TAG, $value);
    $result = end($result);
    $result = trim($result);
    $commentPos = strpos($result, '/*');
    if (false !== $commentPos) {
      $result = substr($result, 0, $commentPos);
    }
    return $result;
  }

  public static function parse($value)
  {
    $result = explode(self::FDML_NEW_LINE, $value);
    $result = array_filter($result, array(__CLASS__, 'isRow'));
    $result = array_map(array(__CLASS__, 'getRowContent'), $result);
    $result = array_filter($result, array(__CLASS__, 'isRowNotEmpty'));
    return $result;
  }

  public static function getRaw($source)
  {
    $source = str_replace("\r", '', $source); // ignore \r
    $source = str_replace("\t", ' ', $source); // replace \t -> [ ]
    $source = preg_replace('/\n\n+/', "\n", $source); // replace \n\n -> \n
    $source = preg_replace('/[ ][ ]+/', " ", $source); // replace \s\s -> \s
    $source = str_replace("\n", self::FDML_NEW_LINE, $source);
    $result = array();
    $matches = array();
    if (preg_match_all('#/\*\*(.+?)\*/[ ]*(?:' . self::FDML_NEW_LINE . ')+[ ]*(?:public[ ]){1}[ ]*function[ ][ ]*([a-zA-Z][0-9a-zA-Z_]+)[ |\(]#', $source, $matches)) {

      foreach ($matches[1] as $k => $v) {
        $result[$matches[2][$k]] = $v;
        unset($matches[1][$k]);
        unset($matches[2][$k]);
      }
      unset($matches);
      $result = array_filter($result, array(__CLASS__, 'isRow'));
      $result = array_map(array(__CLASS__, 'parse'), $result);
    }
    return $result;
  }

  public static function rcbWhiteSpaceLexico($match)
  {
    return str_replace(' ', self::FDML_WHITE_SPACE, $match[1]);
  }

  public static function rcbAggregateLexico($match)
  {
    return $match[1] . '(' . $match[2] . ')';
  }

  public static function isValidRules($row)
  {
    //define rules
    $r = array();
    $r['input'] = array(
      'size' => array(5, 7, 9, 11, 13),
      'rule5' => array(
        array('input', 'type', 'rw', 'name', '*') // rw = range word , * = any
      ),
      'rule7' => array(
        array('input', 'type', 'rw', 'name', '*', 'label', '*'),
        array('input', 'type', 'rw', 'name', '*', 'limit', '*'),
        array('input', 'type', 'rw', 'name', '*', 'values', '*')
      ),
      'rule9' => array(
        array('input', 'type', 'rw', 'name', '*', 'label', '*', 'limit', '*'),
        array('input', 'type', 'rw', 'name', '*', 'label', '*', 'description', '*'),
        array('input', 'type', 'rw', 'name', '*', 'values', '*', 'label', '*'),
        array('input', 'type', 'rw', 'name', '*', 'values', '*', 'limit', '*')
      ),
      'rule11' => array(
        array('input', 'type', 'rw', 'name', '*', 'values', '*', 'label', '*', 'limit', '*'),
        array('input', 'type', 'rw', 'name', '*', 'values', '*', 'label', '*', 'description', '*')
      ),
      'rule13' => array(
        array('input', 'type', 'rw', 'name', '*', 'values', '*', 'label', '*', 'description', '*', 'limit', '*')
      )
    );

    $r['output'] = array(
      'size' => array(3),
      'rule3' => array(
        array('output', 'type', 'rws') // rws = range word (one or more)
      )
    );

    $r['method'] = array(
      'size' => array(3),
      'rule3' => array(
        array('method', 'allow', 'rws')
      )
    );
    //define rules end.

    $rw = explode(',', self::FDML_RANGE_WORDS);

    $result = false;
    if (is_array($row) && count($row) > 2) {
      list($rule) = $row;
      $rule = strtolower($rule);
      if (false !== strpos(self::FDML_RULES_WORDS, $rule) && isset($r[$rule])) {
        $size = count($row);
        if (in_array($size, $r[$rule]['size'])) {
          $ruleIndex = 'rule' . $size;

          if (array_key_exists($ruleIndex, $r[$rule])) {

            $rulesTotest = $r[$rule][$ruleIndex];
            $testPassed = false;
            foreach ($rulesTotest as $_r) {
              //testing rule
              foreach ($_r as $i => $k) {
                $testPassed = false;
                if ('rw' == $k) {
                  if (in_array(strtolower($row[$i]), $rw)) {
                    $testPassed = true;
                  }
                } elseif ('rws' == $k) {

                  $_tmp = str_replace(self::FDML_WHITE_SPACE, ',', $row[$i]);
                  $_tmp = preg_replace('/,,+/', ',', $_tmp);
                  $_tmp = explode(',', $_tmp);
                  $_passed = true;
                  foreach ($_tmp as $_v) {
                    $_v = strtolower(trim($_v));
                    if (!in_array($_v, $rw)) {
                      $_passed = false;
                      break;
                    }
                  }

                  if ($_passed) {
                    $testPassed = true;
                  }
                } elseif ('*' == $k) {
                  if (!empty($row[$i])) {
                    $testPassed = true;
                  }
                } else {
                  if (strtolower($row[$i]) == $k) {
                    $testPassed = true;
                  }
                }

                if (!$testPassed) {
                  break;
                }
              }

              if ($testPassed) {
                break;
              }
            }
            $result = $testPassed;
          }
        }
      }
    }
    return $result;
  }

  public static function validateLexico(Array $raw)
  {
    $result = array();
    $element = array();

    foreach ($raw as $method => $metadata) {
      $element = array('method' => $method);
      $metaProcessed = array();
      foreach ($metadata as $k => $row) {
        $string = preg_replace_callback('/( limit )(.+?)$/i', array(__CLASS__, 'rcbAggregateLexico'), $row);
        $string = preg_replace_callback('/(method[ ]+allow )(.+?)$/i', array(__CLASS__, 'rcbAggregateLexico'), $string);
        $string = preg_replace_callback('/(ouput[ ]+type )(.+?)$/i', array(__CLASS__, 'rcbAggregateLexico'), $string);
        $string = preg_replace_callback('/"(.+?)"/', array(__CLASS__, 'rcbWhiteSpaceLexico'), $string);
        $string = preg_replace_callback("/'(.+?)'/", array(__CLASS__, 'rcbWhiteSpaceLexico'), $string);
        $string = preg_replace_callback('/\[(.+?)\]/', array(__CLASS__, 'rcbWhiteSpaceLexico'), $string);
        $string = preg_replace_callback('/\((.+?)\)/', array(__CLASS__, 'rcbWhiteSpaceLexico'), $string);
        $tmp = explode(' ', $string);
        if (!self::isValidRules($tmp)) {
          unset($metadata[$k]);
        } else {
          $metaProcessed[$k] = $tmp;
        }
      }
      if (!empty($metadata)) {
        $element['definitions'] = $metadata;
        $element['definitionsProcessed'] = $metaProcessed;
        $result[$element['method']] = $element;
      }
    }
    return $result;
  }

  public static function genNameField($name, $stop = 0)
  {
    $result = $name;
    if ($stop > 20) {
      return $result;
    }
    if (false !== strpos($name, '.')) {
      $tmp = explode('.', $name);
      $f = array_shift($tmp);
      list($s) = $tmp;
      array_shift($tmp);
      $s = trim($s, '][');
      $result = $f . '[' . $s . '].' . implode('.', $tmp);
      $result = trim($result, '.');
      if (false !== strpos($result, '.')) {
        $result = self::genNameField($result, $stop++);
      }
    }
    return $result;
  }

  public static function genInputField($d)
  {
    unset($d[0]);
    $def = array();
    foreach ($d as $k => $v) {
      if ($k % 2 == 1) {
        $key = $v;
      } else {
        $def[$key] = $v;
      }
    }
    $result = array();
    $type = isset($def['type']) ? $def['type'] : 'hidden';
    $name = isset($def['name']) ? $def['name'] : 'undefined';
    $values = isset($def['values']) ? $def['values'] : '';
    $a_tmpname = explode('[]', $name);
    $label = isset($def['label']) ? strtr($def['label'], array(self::FDML_WHITE_SPACE => ' ')) : str_replace('.', ' ', end($a_tmpname));
    $description = isset($def['description']) ? strtr($def['description'], array(self::FDML_WHITE_SPACE => ' ')) : '';
    $limit = isset($def['limit']) ? strtr($def['limit'], array(self::FDML_WHITE_SPACE => ' ')) : '1';

    $limit = trim(preg_replace('/,,+/', ',', str_replace(' ', ',', $limit)), ',');
    $_tmp = explode(',', $limit);
    $required = ('1' == $_tmp[0]) ? 'required' : '';
    $limit = implode(',', $_tmp);

    if (!empty($values)) {
      $values = str_replace(self::FDML_WHITE_SPACE, ',', $values);
      $values = preg_replace('/,,+/', ',', $values);
      $values = explode(',', $values);

      if (false !== strpos($label, ',')) {
        $labels = explode(',', $label);
      } else {
        if (!isset($def['label'])) {
          $labels = array();
        } else {
          $labels = array($label);
        }
      }
    }

    $name = self::genNameField($name);

    if ('radio' == $type) {
      if (!empty($values) && is_array($values)) {
        foreach ($values as $k => $value) {
          $label = (isset($labels[$k])) ? $labels[$k] : $value;
          $result[] = array('type' => $type, 'name' => $name, 'value' => $value, 'label' => $label, 'description' => '', 'limit' => '1', 'required' => '');
        }
      }
    } else {
      $value = '';
      $result[] = array('type' => $type, 'name' => $name, 'value' => $value, 'label' => $label, 'description' => $description, 'limit' => $limit, 'required' => $required);
    }
    return $result;
  }

}