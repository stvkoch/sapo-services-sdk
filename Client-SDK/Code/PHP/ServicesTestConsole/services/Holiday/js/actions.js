(function(jQuery){
  jQuery.fn.extend({

    sfSelect: function(options) {
      var self = this;
      var loading = '<option>aguarde...</option>';
      jQuery(this).attr({
        disable:'disable'
      }).html(loading).bind('change',function(){        
        jQuery().sfForm().resetOutput();
      });      
      
      var defaults = {
        url:'#',
        data : ''
      };

      var opt = jQuery.extend(defaults, options);
      var tmp = opt.url.split('?');
      opt.url  = tmp.shift();
      opt.data = tmp.join();
      tmp = null;

      jQuery.ajax({
        url: opt.url,
        data: opt.data,
        dataType: 'json',
        success: function(response){
          try {
            var output = "";
            if (response.result.ok){
              jQuery(response.result.methods).each(function(i,j){
                output += '<option value="'+j+'">'+j+'</option>'+"\n";
              });
              setTimeout(function(){
                jQuery(self).html(output).attr({
                  disable:''
                }).trigger('change');
              },500);
            } else {           
              output = '<option value="-1"></option>'+"\n";
              jQuery(self.html(output));
              alert(response.result.error);              
            }

          } catch(error){}
        }
      });

      return this;
    },

    buildRequestHandler : function(id,form){
      
      jQuery('#'+id).click(function(){        
        jQuery('#'+form.format.outputRequestID).html('<p class="loading">aguarde<span>.</span><span>.</span><span>.</span></p>').trigger('change'); 
        var data = 'action=build&soap=true&method=' + form.options.method + form.getData();
        jQuery.ajax({
          url: form.options.url,
          data: data,
          dataType: 'text',
          success: function(response){
            response = vkbeautify.xml(response,2);            
            var text = String(response).replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;');
            jQuery('#'+form.format.outputRequestID).html('<pre><code class="xml">'+text+'</code></pre>').trigger('change');
            jQuery('pre code',jQuery('#'+form.format.outputRequestID)).each(function(i, e) {
              hljs.highlightBlock(e)
            });            
          }
        });
      });
      return this;
    },

    sendRequestHandler : function(id,form){
      jQuery('#'+id).click(function(){        
        jQuery('#'+form.format.outputResultID).html('<p class="loading">aguarde<span>.</span><span>.</span><span>.</span></p>').trigger('change');  
        var data = 'action=call&soap=true&method=' + form.options.method + form.getData();
        jQuery.ajax({
          url: form.options.url,
          data: data,
          dataType: 'text',
          success: function(response){
            var n = String(response).indexOf('<!-- RESPONSE -->');
            if (n>0){
              var request = String(response).substring(0,n);
              response = String(response).substring(n+17);
              if (request.length>0){
                request = vkbeautify.xml(request,2);            
                var text = String(request).replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;');
                var _tmpText = new Array();
                _tmpText[0] = '';
                _tmpText[1] = text;
                if ( 0 < text.indexOf('&lt;?xml ')){
                  _tmpText = text.split('&lt;?xml ');
                  _tmpText[0] = String(_tmpText[0]).replace(/(\n\n+)|(\s\s+)$/,"\n");                
                  _tmpText[1] = _tmpText[1].replace(/\n\n+/,"\n");
                  if (-1 == _tmpText[1].indexOf('&lt;?xml ')){
                    _tmpText[1] = '&lt;?xml '+_tmpText[1];
                  }
                }
                jQuery('#'+form.format.outputRequestID).html('<pre><small>'+_tmpText[0]+'</small><code class="xml">\n'+_tmpText[1]+'</code></pre>').trigger('change');
                jQuery('pre code',jQuery('#'+form.format.outputRequestID)).each(function(i, e) {
                  hljs.highlightBlock(e)
                });            
              }
            }            
            response = vkbeautify.xml(response,2);
            var text = String(response).replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;');
            var _tmpText = new Array();
                _tmpText[0] = '';
                _tmpText[1] = text;
                if ( 0 < text.indexOf('&lt;?xml ')){
                  _tmpText = text.split('&lt;?xml ');
                  _tmpText[0] = String(_tmpText[0]).replace(/(\n\n+)|(\s\s+)$/,"\n");                
                  _tmpText[1] = _tmpText[1].replace(/\n\n+/,"\n");                  
                  if (-1 == _tmpText[1].indexOf('&lt;?xml ')){                    
                    _tmpText[1] = '&lt;?xml '+_tmpText[1];
                  }
                }
            jQuery('#'+form.format.outputResultID).html('<pre><small>'+_tmpText[0]+'</small><code class="xml">\n'+_tmpText[1]+'</code></pre>').trigger('change');
            jQuery('pre code',jQuery('#'+form.format.outputResultID)).each(function(i, e) {
              hljs.highlightBlock(e)
            });
          }
        });
      });
      return this;
    },

    sfForm: function(options) {
    
      
      this.getData = function (){

        var data = '', name = '', type = '', value='';
        jQuery('input[type!=submit]',this).each(function(i,v){
          name = jQuery(v).attr('name');
          type = jQuery(v).attr('type');
          value = jQuery(v).val();
          if (('text' == type) || ('password' == type) || ('hidden' == type)){
            if ('' != value) {
              data += '&'+name + '=' + encodeURIComponent(value);
            } else {
              if ('required' == jQuery(this).attr('data-use')){
                data += '&'+name + '=';
              }
            }
          } else {
            if ('radio' == type){
              if (jQuery(v).is(':checked')){
                value = jQuery(v).val();
                data += '&'+name + '=' + encodeURIComponent(value);
              }
            } else {
              if ('checkbox' == type){
                value = jQuery(v).is(':checked') ? 'true' : 'false';
                data += '&'+name + '=' + encodeURIComponent(value);
              }
            }
          }

        });
        
        jQuery('textarea',this).each(function(i,v){
              name = jQuery(v).attr('name');
              value = jQuery(v).val();
              data += '&'+name + '=' + encodeURIComponent(value);
        });
        
        return data;

      };
      
      this.resetOutput = function(){
        jQuery('#'+this.format.outputResultID).html(' ');
        jQuery('#'+this.format.outputRequestID).html(' ');
      };
      
      this.format = {

        buildRequestID : 'buildRequestTrigger',
        sendRequestID  : 'sendRequestTrigger',
        outputResultID : 'result',
        outputRequestID : 'request',     

        Input : function(inputs){

          var output = '', className = '', id = 'InputField';
          jQuery(inputs).each(function(i,v){
            className = 'label' + String(v.type).charAt(0).toUpperCase() + String(v.type).slice(1);
            if (('radio' == v.type) || ('checkbox' == v.type)){
              output += '<div class="'+className+'"><label><input type="'+v.type+'" name="'+v.name+'" value="'+v.value+'" data-limit="" data-use="'+v.required+'">'+v.label+'</label></div>';
            }
            else {
              
              output += '<div class="'+className+'">';              
              if ('required' == v.required){
                output += '<div class="needed"></div>';
              } else {
                output += '<div class="notneeded"></div>';
              }
              if ('textarea' == v.type){
                output += '<label id="'+id+i+'">'+v.label+': <br><textarea name="'+v.name+'" data-limit="'+v.limit+'" data-use="'+v.required+'">'+v.value+'</textarea></label>';
              } else {
                output += '<label id="'+id+i+'"><input type="'+v.type+'" name="'+v.name+'" value="'+v.value+'" data-limit="'+v.limit+'" data-use="'+v.required+'">'+v.label+'</label>';
              }
              
              if (v.description.length>0){
                output += '<span data-description="'+encodeURIComponent(v.description)+'" class="info">i</span>';
              }

              var alimit = v.limit.split(',');
              if (alimit.length>1){
                if((alimit[1] == 'n') || (parseInt(alimit[1])>1)){
                  output +=' <a id="copy'+id+i+'" class="button green small copy">+</a>';
                }
              }              
              output += '</div>';
            }
          });

          if ('' != output){
            output += '<div class="fix"></div><br><div class="trigger">';
            output += '<a id="'+this.buildRequestID+'" class="button blue">construir pedido</a> ';
            output += '<a id="'+this.sendRequestID+'" class="button green">Enviar</a></div>';
          }

          return output;
        }

      };

      this.submit(function(event){
        event.preventDefault();
      });

      var self = this;
      var defaults = {
        method:'',
        url: ServiceEndpoint,
        data : 'action=define'          
      };
        
      if ('undefined' == typeof options){
        return this;
      }
      
      var opt = jQuery.extend(defaults, options);
      this.options = opt;
      opt.data +='&method='+opt.method;
     
      jQuery.ajax({
        url: opt.url,
        data: opt.data,
        dataType: 'json',
        success: function(response){
          try {
            var output = "";
            if (response.result.ok){              
              self.html(self.format.Input(response.result.input))
              .buildRequestHandler(self.format.buildRequestID,self)
              .sendRequestHandler(self.format.sendRequestID,self);
            } else {
              alert(response.result.error);
            }
          } catch(error){}
        }
      });

      return this;
    }

  });
})(jQuery);



jQuery(document).ready(function(jQuery){   
  /* Tabs */      
  jQuery('.resultContent li').each(function(i){
    jQuery(this).addClass('resultContentTab'+i).bind('change',function(){
      jQuery('.resultLabelTab'+i).trigger('click');
    });
  });  
  jQuery('.resultLabel li.resultLabelTab').each(function(i){
    jQuery(this).addClass('resultLabelTab'+i).bind('click',function(){
      if (false == jQuery(this).hasClass('selected')){
        jQuery('.selected',jQuery(this).parent()).removeClass('selected');                  
        jQuery(this).addClass('selected');
        jQuery('.resultContent .selected').removeClass('selected');
        jQuery('.resultContentTab'+i).addClass('selected');
      }
    });
  });
        
      
  /* Action */    
  jQuery('#methodSelect').sfSelect({
    url: ServiceEndpoint+'&action=define&list=methods'
  }).bind('change',function(){
    jQuery('#methodForm').sfForm({
      'method' : jQuery('option:selected',this).val()
    });
  });
  
  
  /* Tips */
  jQuery('span.info').live('click',function(){
    jQuery('#tip').remove();
    jQuery(this).after('<span id="tip">'+decodeURIComponent(jQuery(this).attr('data-description'))+'</span>');
    jQuery('#tip').click(function(){
      jQuery(this).remove();
    }).bind('mouseleave',function(){
      jQuery(this).remove();
    });
  });
  
  /* Copy input */
  jQuery('a.copy').live('click',function(){      
     var clone = jQuery(this).parent().clone();
     jQuery('label input',clone).val('');
     jQuery('.copy',clone).remove();
     clone.append('<a class="button small red cut">-</a>');
     clone.insertAfter(jQuery(this).parent());     
  });
  
  /* Cut input */
  jQuery('a.cut').live('click',function(){
     jQuery(this).parent().remove();
  });
  
});

