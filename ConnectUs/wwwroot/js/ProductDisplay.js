$(document).ready(function () {
  
  $(".product-row .product").click(function(){
    $('.product-row .product.checked').not(this).removeClass('checked');
    $(this).toggleClass('checked');

    var productMessage ={
          roomId: $("#roomId").val(),
          productId: $(this).data('id'),
          requestedFrom: window.location.pathname
        };
    $.ajax({
      type: 'POST',
      url: '/api/Message/SendProduct',
      data: JSON.stringify(productMessage),
      dataType: "json",
      contentType:'application/json',
      success: function (result) {

        console.log(result);
        if(result=="Success"){
        
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
})
});

function CreateProductDisplay(productId, page){
  $.ajax({
      type: "POST",
      url: page + '?handler=DisplayProduct',          
      beforeSend: function (xhr) {
          xhr.setRequestHeader("XSRF-TOKEN",$('input:hidden[name="__RequestVerificationToken"]').val());
      },
      data: {productId: productId},
      success: function (result) {
        console.log(result)
        if(result !="Failed"){
          document.querySelector('.product-display img').src = "/uploads/"+result.imagePath;
          document.querySelector('.product-name').innerHTML=result.name;
          document.querySelector('.product_price').innerHTML="$"+result.price.toFixed(2);
          var addToCart = document.querySelector('.product-display a.product-to-buy');
          addToCart.setAttribute('data-id', result.productId);
          document.getElementById('quantity').value=0;
          var video = document.querySelector('.video-display video');
          video.src = "/uploads/"+result.videoPath;
          video.setAttribute("controls","controls"); 
        }
      },
      failure: function () {
          alert("fail: ");
      },
      error: function () {
          alert("error: ");
      }
  });
}