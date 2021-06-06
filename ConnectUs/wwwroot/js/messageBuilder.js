var messageBuilder = function () {
  var message = null;
  var container = null;
  var span = null;
  var messageText = null;

  return {
      createMessage: function (username) {
          message = document.createElement("div")
          
          var classList = ['d-flex', 'mb-3'];

          for (var i = 0; i < classList.length; i++) {
              message.classList.add(classList[i])
          }
          if(sessionStorage.getItem("user")==username){
            message.classList.add('justify-content-end')
          }else{
            message.classList.add('justify-content-start')
          }
      
          return this;
      },
      messageContainer: function (text) {
          container = document.createElement("div")
          container.classList.add('d-flex')
          container.classList.add('flex-column')
          return this;
      },
      withSpan: function (text) {
          span = document.createElement("span")
          span.appendChild(document.createTextNode(text))

          span.classList.add('text-white')

          if(sessionStorage.getItem("user")==text){
            span.classList.add('text-right')
          }

          return this;
      },
      withMessageText: function (text, timestamp) {
          messageText = document.createElement("div")
          messageText.classList.add('msg_cotainer')
          messageText.appendChild(document.createTextNode(text))
          var time =document.createElement("span");
          time.appendChild(document.createTextNode(timestamp))
          time.classList.add('msg_time')
          messageText.appendChild(time)
          return this;
      },
      build: function () {
          message.appendChild(container);
          container.appendChild(span);
          container.appendChild(messageText);
          return message;
      }
  }
}