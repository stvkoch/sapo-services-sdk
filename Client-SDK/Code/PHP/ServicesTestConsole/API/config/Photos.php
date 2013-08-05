<?php

//é utf-8

/**
 * This code is only used for settings and is not used to run the application.
 *
 * @package Services Test Console
 * @author Ricardo Sismeiro <ricardosismeiro@yahoo.co.uk>
 * @version 1.0
 */

class Photos
{

  /**
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"  
   * You can use (ESBUsername and ESBPassword) or ESBToken.
   * describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * 
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   *   
   * @describe input type text name echoStr limit 1 /* string to test
   */
  public function DummyEcho()
  {
    return null;
  }

  /**
   *
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name album.title label "Album Title" limit 1 /*string
   * @describe input type text name album.description label "Album Description" limit 0,1 /*string
   */
  public function AlbumCreate()
  {
    return null;
  }

  /**
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name collection.id label "Collection ID"
   * @describe input type text name collection.user.username label "Collection Username" limit 0,1
   * @describe input type text name page label "Page" limit 0,1
   * @describe input type radio name orderBy values (alfabeticamente, alfabeticamente-inversa, id, maisrecentes, maisantigos) limit 0,1
   *
   */
  public function AlbumGetListByCollection()
  {
    return null;
  }

  /**
   *
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   *
   * @describe input type text name page label "Page" limit 0,1
   * @describe input type  radio name orderBy values (alfabeticamente, maisrecentes, maisantigos, relevance)
   */
  public function AlbumGetListByHighlighted()
  {
    return null;
  }

  /**
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name user.username label "Username" limit 0,1
   * @describe input type text name album.id label "Album ID" limit 1
   */
  public function AlbumDetails()
  {
    return null;
  }

  /**
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name user.username label "Username" limit 0,1
   * @describe input type text name album.id label "Album ID" limit 1
   */
  public function AlbumGetCoverImage()
  {
    return null;
  }

  /**
   * 
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name user.username label "Username" limit 0,1 /*string
   * @describe input type text name page label Page limit 0,1
   * @describe input type radio name orderBy values (alfabeticamente, alfabeticamente-inversa, id, maisrecentes, maisantigos)
   *
   */
  public function AlbumGetListByUser()
  {
    return null;
  }

  /**
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   *
   * @describe input type text name user.username label "Username" limit 0,1
   * @describe input type text name page label "Page" limit 0,1
   * @describe input type radio name orderBy values (alfabeticamente, alfabeticamente-inversa, id, maisrecentes, maisantigos)
   *
   */
  public function CollectionGetListByUser()
  {
    return null;
  }

  /**
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name user.username label "Username" limit 0,1
   * @describe input type text name page label "Page" limit 0,1
   * @describe input type radio name orderBy values (alfabeticamente, alfabeticamente-inversa,id,maisrecentes,maisantigos)
   *
   */
  public function ImageGetListByUser()
  {
    return null;
  }

  /**
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name user.username label "Username" limit 0,1
   * @describe input type text name album.id label "Album ID" limit 1
   * @describe input type text name page label "Page" limit 0,1
   * @describe input type radio name orderBy values (id, recent, alpha, older,inverse_alpha)
   *
   */
  public function ImageGetListByUserAlbum()
  {
    return null;
  }

  /**
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name tags[] label 'Tag' limit 1,n
   * @describe input type text name user.username label "Username" limit 0,1
   * @describe input type text name page label "Page" limit 0,1
   * @describe input type checkbox name m18 label "M18" limit 0,1
   *
   */
  public function ImageGetListByTags()
  {
    return null;
  }

  /**
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name image.uid label "Image UID"
   * @describe input type text name image.albums.[].album.id label "Album ID" limit 1,n
   *
   */
  public function ImageAddToAlbum()
  {
    return null;
  }

  /**
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   *
   * @describe input type text name image.title label "Image Title"
   * @describe input type password name image.password label "Image Password" limit 0,1 /*string
   * @describe input type text name image.subtitle label "Image Subtitle" limit 0,1 /*string
   * @describe input type textarea name image.synopse label "Image Synopse" limit 0,1 /*string
   * @describe input type text name image.tags label "Image Tags" limit 0,1 /*string
   * @describe input type checkbox name image.m18 label "Image M18" limit 0,1 /*boolean
   * @describe input type checkbox name image.active label "Image Active" limit 0,1 /*boolean
   * @describe input type text name image.albums.[].album.id label "Album ID" limit 0,n
   *
   */
  public function ImageCreate()
  {
    return null;
  }

  /**
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name image.uid label "Image UID"
   *
   */
  public function ImageDelete()
  {
    return null;
  }

  /**
   *
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name image.uid label "Image UID"
   * @describe method allow get post
   *
   */
  public function ImageDetails()
  {
    return null;
  }

  /**
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name image.uid label "Image UID"
   * @describe input type text name image.title label "Image Title"  limit 0,1
   * @describe input type textarea name image.synopse label "Image Synopse" limit 0,1
   * @describe input type text name image.tags label "Image Tags" limit 0,1
   * @describe input type password name image.password label "Image Password" limit 0,1
   * @describe input type checkbox name image.active label "Image Active"
   * @describe input type checkbox name image.m18 label "Image M18"
   *
   */
  public function ImageEdit()
  {
    return null;
  }


  /**
   *
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name page limit 0,1
   * @describe input type text name terms[] label "Terms" limit 1,n
   * @describe input type text name dateFrom label "Date from" limit 0,1
   * @describe input type text name dateTo label "Date to" limit 0,1
   * @_describe input type radio name order values (recent,relevance) label "recentes, relevância" limit 0,1
   *
   *
   */
  public function ImageGetListBySearch()
  {
    return null;
  }

  /**
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name image.uid label "Image UID"
   * @describe input type text name image.albums.[].album.id label "Album ID" limit 0,n
   *
   */
  public function ImageRemoveFromAlbum()
  {
    return null;
  }

  /**
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   */
  public function TagsGetListByHighlighted()
  {
    return null;
  }

  /**
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   *
   */
  public function UserDetails()
  {
    return null;
  }

  /**
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   *
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name user.username limit 0,1
   *
   */
  public function UserGetTags()
  {
    return null;
  }

  /**
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name user.email label "E-mail"
   * @describe input type text name user.username label "Username"
   *
   */
  public function UserCreate()
  {
    return null;
  }

  /**
   * @_describe input type text name ESBToken label "Token" description "como obter um <a href='https://store.services.sapo.pt/pt/Catalog/development/free-api-sts/technical-description'>token</a>"
   * @describe input type text name ESBUsername label "ESBUsername"
   * @describe input type password name ESBPassword label "ESBPassword"
   * @describe input type password name ESBAccessKey label "ESBAccessKey"
   * 
   * @describe input type text name page label "Page" limit 0,1
   *
   */
  public function UserGetListByHighlighted()
  {
    return null;
  }

}