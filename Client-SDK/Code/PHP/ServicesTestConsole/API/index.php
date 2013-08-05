<?php

//é utf-8

/**
 * @package Services Test Console
 * @author Ricardo Sismeiro <ricardosismeiro@yahoo.co.uk>
 * @version 1.0
 * 
 */
error_reporting(-1);

//Services List
$serviceWhiteList = array('Photos', 'Videos', 'Alerts', 'Holiday');

if (!extension_loaded('openssl')) {
  echo json_encode(array('result' => array('ok' => false, 'error' => 'Erro: extensão openssl não está carregada!')));
  exit;
}

if (!extension_loaded('soap')) {
  echo json_encode(array('result' => array('ok' => false, 'error' => 'Erro: extensão soap não está carregada!')));
  exit;
}

$_service = !empty($_REQUEST['service']) ? preg_replace('/[^a-zA-Z0-9\-\_]/', '', $_REQUEST['service']) : '';

if (!empty($_service) && in_array($_service, $serviceWhiteList)) {
  define('_CONFIG_SERVICENAME_', $_service);
} else {
  echo json_encode(array('result' => array('ok' => false, 'error' => 'Erro: o serviço não está configurado !')));
  exit;
}

define('_MAIN_DIRECTORY_', dirname(__FILE__));
define('_CONFIG_DIRECTORY_', _MAIN_DIRECTORY_ . '/config');

require_once _MAIN_DIRECTORY_ . '/FDML.php';
require_once _MAIN_DIRECTORY_ . '/WSXml.php';
require_once _MAIN_DIRECTORY_ . '/WSInfo.php';
require_once _MAIN_DIRECTORY_ . '/Handlers.php';

//build, call or define
$action = isset($_REQUEST['action']) ? strtolower($_REQUEST['action']) : '';
$handlerName = sprintf('%sHandler', ucfirst($action));

if (in_array($action, array('build', 'call', 'define'))) {
  $handler = new $handlerName;
  $handler->dispatchRequest();
  exit;
}
