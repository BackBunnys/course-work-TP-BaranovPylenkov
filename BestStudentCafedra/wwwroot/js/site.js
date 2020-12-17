// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    function loadAndShowModal(path) {
        $('#dialogContent').load(path);
        $('#modDialog').modal('show');
    }

    $(document).ready(function () {
        $(':checkbox').on("change", function (e) {
            if ($(this).is(":checked")) {
                if (this.id == "student") {
                    $('input[type=checkbox]').each(function () {
                        this.checked = false;
                    });
                    this.checked = true;
                    $('#dialogContent').load('StudentSelect');
                    $('#modDialog').modal('show');
                }
                else {
                    $('#student').prop('checked', false);
                    if (this.id == "teacher") {
                        $('#dialogContent').load('TeacherSelect');
                        $('#modDialog').modal('show');
                    }
                }
            }
        });
    })

