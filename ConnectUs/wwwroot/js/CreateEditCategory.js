window.addEventListener('load', function () {

  $(".edit-category").click(function(){
      var categoryId = $(this).data('id');
      var categoryName = $(this).data('name');
      $("#categoryEditModalCenter #categoryId").val( categoryId );
      $("#categoryEditModalCenter #categoryName").val( categoryName );
  }); 
  
    if (sessionStorage.getItem('reloadAfterEdit')=="true"){
          sessionStorage.setItem('reloadAfterEdit', "false");
          toastr.success("Successful Edit");
    }else if(sessionStorage.getItem('reloadAfterCreate')=="true"){
          sessionStorage.setItem('reloadAfterCreate', "false");
          toastr.success("Successful Create");
    }
})


function EditCategory(categoryId, categoryName){
  console.log(categoryId)
  console.log(categoryName)
  var category ={
    CategoryId: categoryId,
    CategoryName: categoryName
  }

  if($("#editForm").valid()){
    $.ajax({
          type: "POST",
          contentType: "application/json",
          url:'/admin/manage-categories?handler=EditCategory',          
          beforeSend: function (xhr) {
              xhr.setRequestHeader("XSRF-TOKEN",$('input:hidden[name="__RequestVerificationToken"]').val());
          },
          data: JSON.stringify(category),
          dataType: "json",
          success: function (result) {
              console.log(result)
              if(result=="Success"){
    
                sessionStorage.setItem('reloadAfterEdit', "true"); 
                window.location.reload(true);     
                    
              }
          },
          failure: function () {
              alert("fail: " );
          },
          error: function (xhr, status, error) {
              alert(xhr.responseText);
          }
      });
  }
}

function CreateCategory(category){

  if($("#createForm").valid()){
    $.ajax({
          type: "POST",
          url:'/admin/manage-categories?handler=CreateCategory',          
          beforeSend: function (xhr) {
              xhr.setRequestHeader("XSRF-TOKEN",$('input:hidden[name="__RequestVerificationToken"]').val());
          },
          data: {categoryName: category},
          success: function (result) {
              if(result=="Success"){
                sessionStorage.setItem('reloadAfterCreate', "true");
                window.location.reload(true);
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
}
