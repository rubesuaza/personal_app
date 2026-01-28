window.SweetAlertHelper = {
    showAlert: function (title, text, icon) {
        Swal.fire({
            title: title,
            text: text,
            icon: icon,
            confirmButtonText: 'OK'
        });
    },
    showConfirmation: async function (title, text) {
        const result=await Swal.fire({
            title: title,
            text: text,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes',
            cancelButtonText: 'No'
        });
        return result.isConfirmed;
    }
};
