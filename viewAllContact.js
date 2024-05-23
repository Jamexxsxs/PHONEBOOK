$(document).ready(function () {
    $("#searchBtn").click(function () {
        var searchValue = $("#search").val();

        if (!(/^\d{7}$/.test(searchValue))) {
            alert("Please enter a valid 7-digit phone number.");
            return; 
        }

        $.post('../Home/postSearch',
            {
                search: searchValue
            },
            function (data) {
                $('table tbody').empty();

                if (data.length === 0) {
                    $('table tbody').append('<tr><td colspan="10">No results found.</td></tr>');
                }
                else {
                    data.forEach(function (contact) {
                        var newRow = '<tr>' +
                            '<td>' + contact["CON_NAME"] + '</td>' +
                            '<td>' + contact["CON_AREACODE"] + '</td>' +
                            '<td>' + contact["CON_PHONENUMBER"] + '</td>' +
                            '<td>' + contact["CON_MOBILENUMBER"] + '</td>' +
                            '<td>' + contact["CON_HOUSENUMBER"] + '</td>' +
                            '<td>' + contact["CON_STREET"] + '</td>' +
                            '<td>' + contact["CON_CITY"] + '</td>' +
                            '<td>' + contact["CON_PROVINCE"] + '</td>' +
                            '<td>' + contact["CON_ZIP"] + '</td>' +
                            '<td>' + contact["CON_EMAILADDRESS"] + '</td>' +
                            '</tr>';
                        $('table tbody').append(newRow);
                    })
                }
                $('#backBtn').show();
            }
        )
    })

    $("#backBtn").click(function () {
        location.reload(); 
    });

    $("#submitContact").click(function () {

        var name = $("#name").val().toUpperCase();
        var areacode = $("#areacode").val().substr(0, 3);
        var phonenumber = $("#phonenumber").val().substr(0, 7);
        var mobilenumber = $("#mobilenumber").val().substr(0, 11);

        if (areacode.length !== 3) {
            alert('Area code must be 3 digits.');
            return;
        }

        if (phonenumber.length !== 7) {
            alert('Phone number must be 7 digits.');
            return;
        }

        if (mobilenumber.length !== 11) {
            alert('Mobile number must be 11 digits.');
            return;
        }

        $.post('../Home/postAddContact',
            {
                name: name,
                areacode: areacode,
                phonenumber: phonenumber,
                mobilenumber: mobilenumber,
                housenumber: $("#housenumber").val(),
                street: $("#street").val(),
                city: $("#city").val(),
                province: $("#province").val(),
                zipcode: $("#zipcode").val(),
                emailaddress: $("#emailaddress").val()
            },
            function (data) {
                if (data[0].mess == 1) {
                    var newRow = '<tr>' +
                        '<td>' + $("#name").val().toUpperCase() + '</td>' +
                        '<td>' + $("#areacode").val() + '</td>' +
                        '<td>' + $("#phonenumber").val() + '</td>' +
                        '<td>' + $("#mobilenumber").val() + '</td>' +
                        '<td>' + $("#street").val() + '</td>' +
                        '<td>' + $("#name").val() + '</td>' +
                        '<td>' + $("#city").val() + '</td>' +
                        '<td>' + $("#province").val() + '</td>' +
                        '<td>' + $("#zipcode").val() + '</td>' +
                        '<td>' + $("#emailaddress").val() + '</td>' +
                        '</tr>';
                    $('table tbody').append(newRow);
                    $('#addProductModal').modal('hide');
                }
                else {
                    alert('Phone Number Exist')
                }
            }
        )
    })
})