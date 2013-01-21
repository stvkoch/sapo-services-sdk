<?php

/**
* Get credentials data for access service. This is sensitive data. Not save in public repository
*/
$credencials = require('config/credencials.php');

$soapHeaders = array();
$soapHeaders[] = new SoapHeader('http://services.sapo.pt/definitions', 'ESBCredentials', array('ESBCredentials'=>array('ESBUsername'=>$credencials['ESBUsername'], 'ESBPassword'=>$credencials['ESBPassword'])));
$soapHeaders[] = new SoapHeader('http://services.sapo.pt/Metadata/Market', 'ESBAccessKey', $credencials['ESBAccessKey'] );


/**
* PHP native SOAP client. http://php.net/manual/en/class.soapclient.php
*/
$cinemaClient = new SoapClient("wsdl/Cinema.wsdl", array('exceptions'=>true, 'trace' => $credencials['debug'] ));


/**
* @API, https://store.services.sapo.pt/en/Catalog/agenda/agenda-cinema/technical-description
* Operations related to obtaining points of interest by giving several parameters: geographical coordinates, free text, a location, a zip code, phone number, etc.
*/

/*
* @GetMoviesWithShowTimes
*	- Listar filmes com horários de exibição dentro dos locais e período de tempo. ReleaseCountryId default = PT; LocationIds do ReleaseCountry;  DateTime StartDate default = última quinta-feira; EndDate default = próxima quinta-feira; PageNumber default = 1; RecordsPerPage default = 10, Max itens = 20;
*/
try {
	$response = $cinemaClient->__soapCall( 
		'GetMoviesWithShowTimes' ,  //function name
		array( 'PageNumber'=>1,'RecordsPerPage'=>5 ), //operations args
		null, //options call soap client
		$soapHeaders //request soap header
	);
} catch (SoapFault $e) {
	if($credencials['debug']){
		var_dump($cinemaClient->__getLastRequestHeaders());
		var_dump($e);
	}	
}

var_dump($response);


/*
* @GetGenres
*	- Consultar lista de géneros de filmes. PageNumber defaults to 1; RecordsPerPage defaults to 10;
*/
$response = $cinemaClient->__soapCall( 
	'GetGenres' , 
	array( 'PageNumber'=>1, 'RecordsPerPage'=>20 ), 
	null, $soapHeaders
);
var_dump($response);

/*
* @GetContributorRoles
*	- Listar tipos de contribuições. PageNumber defaults to 1; RecordsPerPage defaults to 1;
*/


/*
* @GetTheaters
*	- Lista de salas de cinema na localização, com quantidade de filmes em exibição.
*/

/*
* @GetTheatersWithShowTimes
*	- Listar filmes dentro dos locais com horários de exibição dentro do período de tempo. ReleaseCountryId default = PT; LocationIds do ReleaseCountry; StartDate default = última quinta-feira; EndDate default = próxima quinta-feira; IncludeLocation filtra elemento location default = true; IncludeShowTimeDetails filtra elemento sessions default = true;  PageNumber default = 1; RecordsPerPage default = 10, Max itens 20;
*/


/*
* @GetPersons
*	- Listar filmes de acordo com os filtros (paginado). IncludeReleaseWithCountryId default = PT; PageNumber default = 1; RecordsPerPage default = 10, Max itens = 20;
*
*
* @GetRandomMovieQuote
*	- Retorna uma citação aleatória de uma personagem de um filme
*
*
*
*
*
*/
