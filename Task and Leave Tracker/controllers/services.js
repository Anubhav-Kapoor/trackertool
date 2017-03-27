var app = angular.module('myApp', []);
// HTTP Factory Service
app.factory('httpService', function ($http) {

    return {

        getData: function (postData, callback) {

            var PostJson = {};
            PostJson.serviceURL = postData;

            return $http({

                method: "POST",
                async: true,
                url: "/AjaxCaller.aspx/ProxyGetMethod",
                data: JSON.stringify(PostJson)

            }).then(function successCallback(response) {
                var realData = JSON.parse(response.data.d);

            }
                );

        }

    };

});

//Create Custom directive for Header
app.directive('header', function () {
    return {
        template: "<div class='row'><div class='col-12'><img src='images/logo.png' width='42' height='42' style='margin-left: 20px;' /><h1 class='h1' style='display: inline-block;'>my Daily Planner</h1><div style='padding-left:70px'><div id='typed-strings'><span class='type-animation'>Plan your Digital day out here....</span></div><span id='typed' style='white-space:pre;'></span></div></div></div><hr />"
    };
});




app.controller('sampleController', function ($scope, $http, httpService, $interval) {

    


    $interval(function () {
        $scope.time = moment().format('MMMM Do YYYY, h:mm:ss a');
    }, 1000);

    $(function () {

        $("#typed").typed({
            stringsElement: $('#typed-strings'),
            typeSpeed: 30,
            backDelay: 500,
            loop: true,
            contentType: 'html', // or text
            // defaults to false for infinite loop
            loopCount: false,
            //callback: function () { foo(); },
            resetCallback: function () { newTyped(); }
        });

        $(".reset").click(function () {
            $("#typed").typed('reset');
        });

    });

    //Table initialization
    var dataSet = [
  ["1", "Fill Appraisal", "Pending", "12-03-2018", ""],
  ["2", "Fill Timesheet", "In Progress", "30-01-2023", ""],
  ["3", "Onsite Travel", "Completed", "16-03-2019", ""],

    ];
    var self = this;


    //Data Table Plugin
    $(document).ready(function () {

        //Begin of Sign Up Form-001


        $('#contact_form').bootstrapValidator({
            // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                first_name: {
                    validators: {
                        stringLength: {
                            min: 2,
                        },
                        notEmpty: {
                            message: 'Please supply your first name'
                        }
                    }
                },


                role_id: {
                    validators: {
                        stringLength: {
                            min: 3,
                        },
                        notEmpty: {
                            message: 'Please supply your role id'
                        }
                    }
                },
                ntid_name: {
                    validators: {
                        stringLength: {
                            min: 2,
                        },
                        notEmpty: {
                            message: 'Please supply your NTID'
                        }
                    }
                },
                password: {
                    validators: {
                        stringLength: {
                            min: 5,
                        },
                        notEmpty: {
                            message: 'Please supply your password(min 5 characters)'
                        }
                    }
                },
                confpassword: {
                    validators: {
                        stringLength: {
                            min: 5,
                        },
                        notEmpty: {
                            message: 'Please supply your confirm pasword(same as password above)'
                        },
                        identical: {
                            field: 'password',
                            message: 'The password and its confirm are not the same'
                        }
                    }
                },
                email: {
                    validators: {
                        notEmpty: {
                            message: 'Please supply your email address'
                        },
                        emailAddress: {
                            message: 'Please supply a valid email address'
                        }
                    }
                },
                phone: {
                    validators: {
                        notEmpty: {
                            message: 'Please supply your phone number'
                        },
                        phone: {
                            country: 'IN',
                            message: 'Please supply a vaild phone number'
                        }
                    }
                }
            }
        })
        .on('success.form.bv', function (e) {
            $('#success_message').slideDown({ opacity: "show" }, "slow") // Do something ...
            $('#contact_form').data('bootstrapValidator').resetForm();

            // Prevent form submission
            e.preventDefault();

            // Get the form instance
            var $form = $(e.target);

            // Get the BootstrapValidator instance
            var bv = $form.data('bootstrapValidator');
        });
        //End of Sign Up form

        $('#myTable').DataTable({
            data: dataSet,
            columns: [
                { title: "S.No" },
                { title: "Task Description" },
                { title: "Status" },
                { title: "Completion Date" },
                { title: "Command" }
            ],
            "columnDefs": [{
                "targets": -1,
                "data": null,
                "defaultContent": "<button class='button' id='edit' style='border-radius: 5px;'>Edit</button><button class='button' id='delete' style='margin-left: 10px;border-radius: 5px;'>Delete</button><button class='button' id='done'style='margin-left: 10px;border-radius: 5px;'>Done</button>"
            }]
        });
        $('#myTable tbody').on('click', "#edit", function () {
            //var data = table.row($(this).parents('tr')).data();
            //alert('Hi Edit');
        });
        $('#myTable tbody').on('click', "#delete", function () {
            //var data = table.row($(this).parents('tr')).data();
            //alert('Hi Delete');
        });
        $('#myTable tbody').on('click', "#done", function () {
            //var data = table.row($(this).parents('tr')).data();
            //alert('Hi Done');
        });
    });



    //Ajax method 
    $http({
        method: "GET",
        url: "/SignUp.aspx/GetUserDetails",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false
    }).then(function mySucces(response) {

        console.log(response);

    }, function myError(response) {
        console.log(response);
    });

    $scope.createAccount = function () {
        if ($scope.password == $scope.confPassword) {


            var user = {
                ntid: $scope.ntid,
                firstName: $scope.firstName,
                lastName: $scope.lastName,
                roleId: $scope.roleId,
                phone: $scope.phoneNo,
                email: $scope.emailId,
                password: $scope.password,


            }
        
            //Ajax method 
            $http({
                method: "POST",
                url: "/SignUp.aspx/CreateAccount",
                data: JSON.stringify(user),
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false
            }).then(function mySucces(response) {

            }, function myError(response) {

            });

            console.log(user.toString());
        }
        else {
            console.log("Passwords do not match");
        }
    }







}



    );