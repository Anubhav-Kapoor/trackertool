var app = angular.module('myApp', ['ngCookies']);
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

jQuery.fn.extend({
    disable: function(state) {
        return this.each(function() {
            var $this = $(this);
            $this.toggleClass('disabled', state);
        });
    }
});

//*******************Home Controller - Used for Home Page*******************//

app.controller('homeCtrl', function ($scope, $http, httpService, $interval, $cookies) {

    $(function () {

        var dateObject = new Date();

        var createdDate = dateObject.toUTCString();

        var taskMinStartDate = moment().format('YYYY-MM-DD');;




        //Sub-Title Typing Plugin Initialization
        $("#typed").typed({
            stringsElement: $('#typed-strings'),
            typeSpeed: 30,
            backDelay: 500,
            loop: true,
            contentType: 'html',
            loopCount: false,
            resetCallback: function () { newTyped(); }
        });

        /******************************** HOME PAGE CODE *********************************/

        //Table initialization
        var dataSet = [
            ["1", "Fill Appraisal", "Pending", "12-03-2018", ""],
            ["2", "Fill Timesheet", "In Progress", "30-01-2023", ""],
            ["3", "Onsite Travel", "Completed", "16-03-2019", ""],
        ];
        var self = this;

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

        /******************************** CREATE NEW TASK CODE *********************************/

        // Display Popup - Create Task
        $scope.createTaskPopup = function () {
            $scope.isTaskFormValid = true;
            var options = {
                "backdrop": "static"
            }
            $('#createTaskModal').modal(options);
        }

        // Form Validation - Task Form 
        $('#taskForm').bootstrapValidator({
            // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                taskName: {
                    validators: {
                        stringLength: {
                            min: 5,
                            max: 50
                        },
                        notEmpty: {
                            message: 'Please supply task name'
                        }
                    }
                }, taskDesc: {
                    validators: {
                        stringLength: {
                            min: 5,
                            max: 150
                        },
                        notEmpty: {
                            message: 'Please supply task description'
                        }
                    }
                },
                taskStartDate: {
                    validators: {
                        stringLength: {
                            max: 10,
                        },
                        date: {
                            format: 'MM/DD/YYYY',
                            message: 'The value is not a valid date'

                        },
                        callback: {
                            message: 'The date is not in the range',
                            callback: function (value, validator) {
                                var m = new moment(value, 'YYYY-MM-DD', true);
                                if (!m.isValid()) {
                                    return false;
                                }
                                return m.isAfter(taskMinStartDate) || m.isSame(taskMinStartDate);
                            }
                        },
                        notEmpty: {
                            message: 'Please select start date'
                        }
                    }
                },
                taskEndDate: {
                    validators: {
                        stringLength: {
                            max: 10,
                        },
                        date: {
                            format: 'MM/DD/YYYY',
                            message: 'The value is not a valid date'

                        },
                        callback: {
                            message: 'The date is not in the range',
                            callback: function (value, validator) {
                                var m = new moment(value, 'YYYY-MM-DD', true);
                                if (!m.isValid()) {
                                    return false;
                                }
                                return m.isAfter(moment($scope.taskStartDate.toUTCString()).format('YYYY-MM-DD')) || m.isSame(moment($scope.taskStartDate.toUTCString()).format('YYYY-MM-DD'));
                            }
                        },

                        notEmpty: {
                            message: 'Please select end date'
                        }
                    }
                },
                assignedTo: {
                    validators: {
                        notEmpty: {
                            message: 'Please select an associate to assign task'
                        }
                    }
                }
            }
        }).on('success.field.fv', function (e, data) {
            if (data.fv.getInvalidFields().length > 0) {    // There is invalid field
                //data.fv.disableSubmitButtons(true);
                ////$('#createTaskButton').disable(true);
                //$scope.isTaskFormValid = false;
            }
            else {
                ////$('#createTaskButton').disable(false);
                //$scope.isTaskFormValid = true;
            }
        });

        // Function - Create Task
        $scope.createTask = function () {

            if ($scope.isTaskFormValid) {

                var userData = {

                    taskDesc: $scope.taskDesc,
                    expiryDate: moment($scope.taskEndDate.toUTCString()).format('MM/DD/YYYY'),
                    createdBy: sessionStorage.getItem("username"),
                    assignedTo: $scope.assignedTo,
                    status: "In Progress",
                    taskName: $scope.taskName,
                    startDate: moment($scope.taskStartDate.toUTCString()).format('MM/DD/YYYY')

                }

                //Ajax method 

                $http({

                    method: "POST",

                    url: "/TaskManagerAPI.aspx/CreateTask",

                    data: JSON.stringify(userData),

                    cache: false,

                    contentType: "application/json; charset=utf-8",

                    dataType: "json",

                    async: false

                }).then(function mySuccess(response) {

                    $('#createTaskModal').modal("hide");

                    var responseJSON = JSON.parse(response.data.d);
                    $scope.status = responseJSON.Response.Status;
                    $scope.reason = responseJSON.Response.Reason;

                    if ($scope.status == "Success") {
                        $('#statusModal').modal('show');

                    }
                    else if ($scope.status == "Failure")
                        $('#statusModal').modal('show');

                }, function myError(response) {

                    console.log(response);

                });
            }
            else {

                $('#createTaskModal').modal("hide");

                $scope.status = "Incomplete Form";
                $scope.reason = "Please fill the form properly and try again!!!";
                $('#statusModal').modal('show');
            }
            

        }



        /******************************** CHANGE PASSWORD CODE *********************************/

        // Form Validation - Change Password
        $('#change_form').bootstrapValidator({
            // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                currentPwd: {
                    validators: {
                        stringLength: {
                            min: 5,
                        },
                        notEmpty: {
                            message: 'Please enter your current password (Min 5 Chars)'
                        }
                    }
                },
                newPwd: {
                    validators: {
                        stringLength: {
                            min: 5,
                        },
                        notEmpty: {
                            message: 'Please enter your new password (Min 5 Chars)'
                        }
                    }
                },
                confirmNewPwd: {
                    validators: {
                        stringLength: {
                            min: 5,
                        },
                        notEmpty: {
                            message: 'Re-enter your new password (Min 5 Chars)'
                        },
                        identical: {
                            field: 'newPwd',
                            message: 'Passwords do not match. Please check!!!'
                        }
                    }
                }
            }
        }).on('success.field.fv', function (e, data) {
            if (data.fv.getInvalidFields().length > 0) {    // There is invalid field
                data.fv.disableSubmitButtons(true);
            }
        });

        // Function - Change Password
        $scope.changePassword = function () {

            var userData = {
                ntid: sessionStorage.getItem('username'),
                currentPassword: $scope.currentPwd,
                newPassword: $scope.newPwd
            }

            //Ajax method 
            $http({
                method: "POST",
                url: "/TaskManagerAPI.aspx/ChangePassword",
                data: JSON.stringify(userData),
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false
            }).then(function mySuccess(response) {

                var responseJSON = JSON.parse(response.data.d);
                console.log("Reason: " + responseJSON.Response.Reason);
                $scope.status = responseJSON.Response.Status;
                console.log(response);
                //  $('#myModal').modal("show");

                if ($scope.status == "Success") {
                    window.location.href = "SignIn.aspx";
                }
            }, function myError(response) {
                console.log(response);
            });
        }
    });

});

//******************* Login Controller *******************//

app.controller('loginCtrl', function ($scope, $http, httpService, $interval, $cookies) {


    $(function () {

        //On Load of every page 
        $scope.loadPage = function () {
            if (sessionStorage.getItem('username') == null || sessionStorage.getItem('username') == undefined) {
                $scope.GoToURL('SignIn.aspx');
            }
        }

        //Sub-Title Typing Plugin Initialization
        $("#typed").typed({
            stringsElement: $('#typed-strings'),
            typeSpeed: 30,
            backDelay: 500,
            loop: true,
            contentType: 'html',
            loopCount: false,
            resetCallback: function () { newTyped(); }
        });

        $(".reset").click(function () {
            $("#typed").typed('reset');
        });

        /**************************** SIGN-IN CODE ***************************************/

        // Form Validation - Sign-In 
        $('#login_form').bootstrapValidator({
            // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
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
                }
            }
        }).on('success.field.fv', function (e, data) {
            if (data.fv.getInvalidFields().length > 0) {    // There is invalid field
                data.fv.disableSubmitButtons(true);
            }
        });

        // Function - Sign-In 
        $scope.login = function () {

            var user = {
                ntid: $scope.ntid,
                password: $scope.password,
            }

            //Ajax method 
            $http({
                method: "POST",
                url: "/TaskManagerAPI.aspx/Login",
                data: JSON.stringify(user),
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false
            }).then(function mySuccess(response) {

                var responseJSON = JSON.parse(response.data.d);

                $scope.status = responseJSON.Response.Status;
                $scope.reason = responseJSON.Response.Reason;

                if ($scope.status == "Success") {
                    //Save NTID in session storage
                    sessionStorage.setItem('username', $scope.ntid);
                    window.location.href = "taskPage.aspx";
                }
                else if ($scope.status == "Failure") {

                    $("#statusModal").modal("show");

                }
            }, function myError(response) {
                console.log(response);
            });

            console.log(user.toString());
        }

        /**************************** FORGET PASSWORD CODE ***************************************/

        // Display Popup - Forgot password 
        $scope.forgotPwdPopUp = function () {

            var options = {
                "backdrop": "static"
            }
            $('#forgotPwd').modal(options);
        }


        // Form Validation - Forget Password
        $('#forgot_form').bootstrapValidator({
            // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                ntid_name: {
                    validators: {
                        stringLength: {
                            min: 2,
                        },
                        notEmpty: {
                            message: 'Please supply your NTID'
                        }
                    }
                }
            }
        }).on('success.field.fv', function (e, data) {
            if (data.fv.getInvalidFields().length > 0) {    // There is invalid field
                data.fv.disableSubmitButtons(true);
            }
        });

        // Function - Forgot Password 
        $scope.sendPwd = function () {

            var user = {
                ntid: $scope.ntid,
            }

            //Ajax method 
            $http({
                method: "POST",
                url: "/TaskManagerAPI.aspx/ForgotPassword",
                data: JSON.stringify(user),
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false
            }).then(function mySuccess(response) {

                var responseJSON = JSON.parse(response.data.d);
                console.log("Reason: " + responseJSON.Response.Reason);
                $scope.status = responseJSON.Response.Status;
                console.log(response);
                $('#myModal').modal("show");

            }, function myError(response) {
                console.log(response);
            });

            console.log(user.toString());

        }

        /**************************** SIGN-UP CODE ***************************************/

        $('#register_form').bootstrapValidator({

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
                }, last_name: {
                    validators: {
                        stringLength: {
                            min: 2,
                        },
                        notEmpty: {
                            message: 'Please supply your last name'
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
        }).on('success.field.fv', function (e, data) {
            if (data.fv.getInvalidFields().length > 0) {    // There is invalid field
                data.fv.disableSubmitButtons(true);
            }
        });

        // Function - Sign-Up 
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
                    url: "/TaskManagerAPI.aspx/CreateAccount",
                    data: JSON.stringify(user),
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false
                }).then(function mySuccess(response) {

                    var resp = JSON.parse(response.data.d);

                    //Case of NTID already existing
                    if (resp.Response.Status == 'Fail') {
                        $scope.message = resp.Response.Reason;
                        $scope.isSuccess = false;
                    } else {
                        $scope.message = resp.Response.Reason;
                        $scope.isSuccess = true;
                    }
                    //Successful creation of Account Message
                    var options = {
                        "backdrop": "static"
                    }
                    $('#basicModal').modal(options);

                    //Redirect to Sign In page after a specific time interval 
                    setTimeout(function () {
                        window.location.href = "SignIn.aspx";
                    }, 5000);
                }), function myError(response) {
                    console.log(response);
                }
            }
            else {
                console.log("Passwords do not match");
            }
        }



        //Ajax method - To Get User Details
        $http({
            method: "GET",
            url: "/TaskManagerAPI.aspx/GetUserDetails",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false
        }).then(function mySucces(response) {

            //Ajax method - To Get User Details
            $http({
                method: "GET",
                url: "/TaskManagerAPI.aspx/GetUserDetails",
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false
            }).then(function mySucces(response) {

                console.log(response);

            }, function myError(response) {
                console.log(response);
            });



        });

    });



});// Login Controller Ends Here


/******************************** UTILITY FUNCTIONS *********************************/

//Navigate to Page
function goToURL(navigatePage) {
    if (navigatePage != null && navigatePage != '')
        window.location.href = navigatePage;
}

