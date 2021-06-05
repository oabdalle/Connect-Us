$(document).ready(function () {


    var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();


    connection.on("RecieveMessage", function (data) {
    console.log(data);
    var message = messageBuilder()
        .createMessage(data.name)
        .messageContainer()
        .withSpan(data.name)
        .withMessageText(data.text, data.timestamp)
        .build();
    console.log(sessionStorage.getItem("user"))
    document.querySelector('.card-body').append(message);
    })

    connection.on("RecieveLikes", function (data) {
    console.log(data);
    $(".like-count").text(data.totalLikes);
    })

    connection.on("RecieveProductMessage", function (data) {
    console.log(data);
    CreateProductDisplay(data.text, data.requestedFrom);
    })

    connection.start()
    .then(function () {
      connection.invoke('joinRoom', $("#roomId").val());
    })
    .catch(function (err) {
        console.log(err)
    })

    window.addEventListener('onunload', function () {
    connection.invoke('leaveRoom', $("#roomId").val());
    })

});