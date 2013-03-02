<?php

/**
* Get credencials data to access service. This is sensive data. Not save in public repository
*/
$credencials = include('config/credencials.php');

$soapHeaders = array();
$soapHeaders[] = new SoapHeader('http://services.sapo.pt/definitions', 'ESBCredentials', array('ESBUsername'=>$credencials['ESBUsername'], 'ESBPassword'=>$credencials['ESBPassword']));
$soapHeaders[] = new SoapHeader('http://services.sapo.pt/Metadata/Market', 'ESBAccessKey', $credencials['ESBAccessKey'] );


/**
* PHP native SOAP client. http://php.net/manual/en/class.soapclient.php
*/
$cinemaClient = new SoapClient("wsdl/Cinema.wsdl", array('exceptions'=>true, 'trace' => $credencials['debug'] ));
$cinemaClient->__setSoapHeaders($soapHeaders);

/**
* @API, https://store.services.sapo.pt/en/Catalog/agenda/agenda-cinema/technical-description
* Operations related to obtaining points of interest by giving several parameters: geographical coordinates, free text, a location, a zip code, phone number, etc.
*/

/*
* @GetMoviesWithShowTimes
*	- Listar filmes com horários de exibição dentro dos locais e período de tempo. 
*		ReleaseCountryId default = PT; 
*		LocationIds do ReleaseCountry;  
*		DateTime StartDate default = última quinta-feira; 
*		EndDate default = próxima quinta-feira; 
*		PageNumber default = 1; 
*		RecordsPerPage default = 10, Max itens = 20;
*/
try {
/*
	//outra forma de passar argumentos
	class GetMoviesWithShowTimesArgs extends stdClass{}
	$arg = new GetMoviesWithShowTimesArgs();
	$arg->PageNumber = 1;
	$arg->RecordsPerPage = 3;
	$response = $cinemaClient->__soapCall(
		'GetMoviesWithShowTimes',
		array( $arg )
	);
*/

	$response = $cinemaClient->__soapCall( 
		'GetMoviesWithShowTimes' ,  //function name
		//note que os argumentos estão encapsulados por um array,
		//a definição WSDL exige que o argumento seja um objeto 'GetMoviesWithShowTimes' com os paramentros opcionais... (ver WSDL)
		array( array('RecordsPerPage' => 1, 'PageNumber' => 10) )
	);

} catch (SoapFault $e) {
	if($credencials['debug']){
		var_dump($e);
	}
}


//show last SOAP request sended by GetMoviesWithShowTimes
echo "-----------------------\n";
echo "GetMoviesWithShowTimesn\n";
echo "-----------------------\n";

echo ($cinemaClient->__getLastRequest());
echo "-----------------------\n\n";
echo ($cinemaClient->__getLastResponse());


try {
	/*
	* @GetGenres
	*	- Consultar lista de géneros de filmes. PageNumber defaults to 1; RecordsPerPage defaults to 10;
	*/
	//quando chamado a função diretamente não é preciso o encapsulamento dos argumentos 
	$response = $cinemaClient->GetGenres( array( 'PageNumber'=>1, 'RecordsPerPage'=>20 ) );

} catch (Exception $e) {
	
}
echo "-----------------------\n";
echo "GetGenres\n";
echo "-----------------------\n";
//show last SOAP request sended by GetMoviesWithShowTimes
echo ($cinemaClient->__getLastRequest());
echo "-----------------------\n\n";
echo ($cinemaClient->__getLastResponse());




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