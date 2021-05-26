$(document).ready(function () {
  $('#quantity').on('change', function(){
        var checkout=false;
        if((window.location.href).includes('checkout')){
          checkout=true;
        }
        var id = $('.product-to-buy').attr("data-id");
        var productDetails ={
          productId : parseInt(id),
          quantity : parseInt($(this).val()),
          atCheckout:checkout          
        };
        console.log(productDetails)
        AddToCart(productDetails);
  });

  $('.product-to-buy').click(function(event)
  {
    var id =$(this).attr("data-id")
    var productDetails ={
    productId : parseInt(id),
    quantity : parseInt(document.getElementById('quantity').value)+1,
    atCheckout:false
  
    };
    AddToCart(productDetails);
  });
});

function AddToCart(productDetails){       
  $.ajax({

      type: 'POST',
      url: '/api/Cart/Buy',
      data: JSON.stringify(productDetails),
      dataType: "json",

      contentType:'application/json',
      success: function (output) {
        if(output.reloadPage==true){
          window.location.reload();
        }
        if(output.result=="Success"){
          var quantity = document.getElementById('quantity');
          quantity.value = output.quantity;
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