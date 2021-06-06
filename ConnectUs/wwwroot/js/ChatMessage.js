$(document).ready(function () {

  $('.like-button').click(function(event)
    {
      var like ={
        roomId: $("#roomId").val(),
        eventId: $("#EventId").val()
      };
      SendAjaxLike(like);
    });

  $("#buy").click(function (event) {
    var chatMessage ={
        roomId: $("#roomId").val(),
        message: "Buy!"
      };
      SendAjaxMessage(chatMessage);
    });


  $("#sell").click(function (event) {
        var chatMessage ={
        roomId: $("#roomId").val(),
        message: "Sell!"
      };
      SendAjaxMessage(chatMessage);
     });
  $("#submitMessage").click(function (event) {
     
      var chatMessage ={
        roomId: $("#roomId").val(),
        message: $("#message").val()
      };

    SendAjaxMessage(chatMessage);
  });                  
});

function SendAjaxMessage(message){
  sessionStorage.setItem("user", $("#currentUser").val());
  $.ajax({
      type: 'POST',
      url: '/api/Message/SendMessage',
      data: JSON.stringify(message),
      dataType: "json",
      contentType:'application/json',
      success: function (result) {

          console.log(result);
        if(result=="Success"){
          $("#message").val('');
        }else if(result=="Not signed in."){
          alert('Not signed in')
        }else if(result=="No message."){
          alert('No chat message.')
        }
      },
      failure: function () {
        alert("fail: " );
      },
      error: function (xhr, status, error) {
        alert('error'+ xhr.responseText);
      }
  });
}
function SendAjaxLike(likeUpdate){
  $.ajax({
      type: 'POST',
      url: '/api/Message/LikeMessageUpdate',
      data: JSON.stringify(likeUpdate),
      dataType: "json",
      contentType:'application/json',
      success: function (result) {

          console.log(result);
        if(result=="Success"){
          $("#message").val('');
        }else if(result=="Not signed in."){
          alert('Not signed in')
        }else if(result=="No message."){
          alert('No chat message.')
        }
      },
      failure: function () {
        alert("fail: " );
      },
      error: function (xhr, status, error) {
        alert('error'+ xhr.responseText);
      }
  });
}