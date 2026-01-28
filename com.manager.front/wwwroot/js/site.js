let wheel;

function initializeWheel(books) {  
    
    let textSize = books.length > 10 ? 12 : 18; 
    let wheelData = books.map((title, index) => ({
        fillStyle: getRandomColor(),
        text: title
    }));

    wheel = new Winwheel({   
        canvasId:'wheelCanvas',
        numSegments: books.length,
        outerRadius: 250, // Ajustado al nuevo tamaño del modal
        textFontSize: textSize, // Aplica tamaño dinámico al texto
       // textOrientation: "curved", // Rotar el texto
        segments: wheelData,
        animation: {
            type: 'spinToStop',
            duration: 8,
            spins: 8,
            callbackFinished: function (indicatedSegment) {
                navigator.clipboard.writeText(indicatedSegment.text)
                    .then(() => {
                        SweetAlertHelper.showAlert("Libro seleccionado: " + indicatedSegment.text);
                        toastr.success('titulo copiado en el portapapeles');
                    })
                    .catch(err => {
                        toastr.error("No se pudo copiar al portapapeles.");
                    });
            }
        }
    });
    console.log(wheel);
}

// Función para obtener colores aleatorios
function getRandomColor() {
    return '#' + Math.floor(Math.random() * 16777215).toString(16);
}

// Girar la ruleta
function spinWheel() {
    if (wheel) {
        wheel.startAnimation();
    }
}
function downloadFileFromBytes(filename, byteBase64) {
    var link = document.createElement("a");
    link.href = "data:application/pdf;base64," + byteBase64;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
