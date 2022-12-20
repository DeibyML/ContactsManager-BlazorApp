function saveAsFile(fileName, bytesBase64) {
    var link = document.createElement('a');
    link.download = fileName;
    link.href = "data:application/octect-stream;base64," + bytesBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);    
}

function customConfirm(title, message, type) {
    return new Promise((resolve) => {
        Swal.fire({
            title: title,
            text: message,
            type: type,
            showDenyButton: true,
            confirmButtonText: 'Delete',
            denyButtonText: 'No, cancel!',
        }).then((result) => {
            if (result.isConfirmed) {
                resolve(true);
            } else if (result.isDenied) {
                resolve(false);
            }
        });        
    })
}