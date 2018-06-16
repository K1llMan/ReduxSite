$(function () {
    // Build file upload
    function readSingleFile(e) {
        var file = e.target.files[0];
        if (!file)
            return;

        var reader = new FileReader();
        reader.onload = function(e) {
            var posting = $.post( ajaxUrl, {
                action: 'upload_builds',
                builds_data: e.target.result
            })
            .done(function(data){
                $('#file-input').val('');
                Materialize.toast('Uploading done!', 2000);
            });
        };
        reader.readAsText(file);
    }

    function download(data, name, type) {
        var str = typeof data == 'object' ? JSON.stringify(data) : data;
        
        var a = document.createElement("a");
        var file = new Blob([str], {type: type});
        a.href = URL.createObjectURL(file);
        a.download = name;
        a.click();
    }
    
    // Download builds data
    function downloadBuilds(){
        var posting = $.post( ajaxUrl, { action: 'download_builds' })
        .done(function(data){
            download(data, 'builds.json', 'text/json');
        });
    }
    
    // Module initialization
    var module = {
        'init': function() {
            var page = $(Templater.useTemplate('builds-page'));
            
            page.find('#file-input').change(readSingleFile);
            page.find('#download').click(downloadBuilds);
            
            $('.main-content').append(page);
            
            // Show table
            context.readyForDisplay(true);            
        }
    }
    
    // Update global module reference
    context.currentModule = module;
}());