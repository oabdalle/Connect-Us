function setAsCover(imgId){
  console.log(imgId)

  $.ajax({
          type: "POST",
          url:'/events/edit-event?handler=SetCover',          
          beforeSend: function (xhr) {
              xhr.setRequestHeader("XSRF-TOKEN",$('input:hidden[name="__RequestVerificationToken"]').val());
          },
          data: {Item1: imgId},
          success: function (result) {
            console.log(result)
  
            $('.edit-images-view').html(result);
           
          },
          failure: function () {
              alert("fail: ");
          },
          error: function () {
              alert("error: ");
          }
      });
  }
function deleteImage(imgId){
  console.log(imgId)

  $.ajax({
          type: "POST",
          url: '/events/edit-event?handler=RemoveImage',         
          beforeSend: function (xhr) {
              xhr.setRequestHeader("XSRF-TOKEN",$('input:hidden[name="__RequestVerificationToken"]').val());
          },
          data: {Item1: imgId},
          success: function (result) {
            console.log(result)
            $('.edit-images-view').html(result);
          },
          failure: function () {
              alert("fail: ");
          },
          error: function () {
              alert("error: ");
          }
      });
  }


  function showDropzoneInForm(){
         $("#addImages").toggle();
         $("#showDropzone").toggle();
         $("#hideDropzone").toggle();
     }
 
   Dropzone.autoDiscover = false;


   $(document).ready(function () {
     console.log("His11!");
    
      $("#dZUpload").sortable({
         items:'.dz-preview',
         cursor: 'move',
         opacity: 0.5,
         containment: '#dZUpload',
         distance: 20,
         tolerance: 'pointer'
     });
     $("#dZUpload").dropzone({
         autoProcessQueue: false,
         url: '@Url.Page("EditEvent", "Upload")',
         addRemoveLinks: true,
         acceptedFiles:'image/*',
       init: function() {
 
         dzClosure = this;
         document.getElementById("setup-event").addEventListener("click", async function(e) {
             e.preventDefault();
             e.stopPropagation();
             if(await UpdateFiles(dzClosure.files)){
                  console.log("ending with click")
           
                 $('#post-event').click();
             }
             
         });
       },
         success: function (file, response) {
             
         },
         error: function (file, response) {
         }
     });
 });
     async function UpdateFiles(dzFiles){
         console.log("here")
         newQueue = [];
         var queue = dzFiles;
         console.log(queue)
         await $('#dZUpload .dz-preview .dz-filename [data-dz-name]').each(async function (count, el) {           
             var name = el.innerHTML;
             await queue.forEach(function(file) {
                 if (file.name === name) {
                     newQueue.push(file);
                 }
             });
         });
         dzFiles = newQueue;
         console.log(dzFiles)
     
         let list = new DataTransfer();
         for (var i = 0; i < dzFiles.length; i++) {
             list.items.add(dzFiles[i])
         }
         console.log(list);
         let myFileList = list.files;
         console.log(myFileList)
         console.log(document.getElementById('imgs'));
         
         document.getElementById('imgs').files=myFileList; 
           console.log("input before click");
         console.log(document.getElementById('imgs'));
            console.log(document.getElementById('imgs').files);
          console.log("starting to click");
         return true;
     }
  