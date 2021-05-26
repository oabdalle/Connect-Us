$(document).ready(function () {


    var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();


    connection.on("RecieveMessage", function (data) {
    console.log(data);
    var message = messageBuilder()
        .createMessage()
        .withHeader(data.name)
        .withParagraph(data.text)
        .withFooter(data.timestamp)
        .build();
    document.querySelector('.chat-body').append(message);
    })

    connection.on("RecieveLikes", function (data) {
    console.log(data);
    $(".like-count").text(data.totalLikes);
    })

    connection.on("RecieveProductMessage", function (data) {
    console.log(data);
    CreateProductDisplay(data.text);
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