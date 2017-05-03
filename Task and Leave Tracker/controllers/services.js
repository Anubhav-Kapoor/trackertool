var app = angular.module('myApp', ['ngCookies']);

// Routing 

//app.config(function ($routeProvider) {
//    $routeProvider
//    .when("/", {
//        templateUrl: "SignIn.aspx"
//    })
//    .when("/signin", {
//        templateUrl: "SignIn.aspx"
//    })
//    .when("/signup", {
//        templateUrl: "SignUp.aspx"
//    })
//    .when("/home", {
//        templateUrl: "home.aspx"
//    });
//});


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
    disable: function (state) {
        return this.each(function () {
            var $this = $(this);
            $this.toggleClass('disabled', state);
        });
    }
});

//*******************Home Controller - Used for Home Page*******************//

app.controller('homeCtrl', function ($scope, $rootScope, $http, httpService, $interval, $cookies, $timeout) {

    //Check weather user has already logged 
    $scope.pageInit = function () {
        if (sessionStorage.getItem('username') == null || sessionStorage.getItem('username') == undefined) {
            goToURL('signin');
        }
    }

    $(function () {

        var dateObject = new Date();

        var createdDate = dateObject.toUTCString();



        $scope.isProjectManager = false;
        $scope.taskActions = "";


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

        $scope.leaveTypes = ["Casual Leave", "Earned Leave", "Sick Leave", "Flexi Holiday", "Leave Without Pay"];

        /******************************** HOME PAGE CODE *********************************/

        $scope.loadTaskTable = function (taskTableData) {

            $scope.taskTable = $('#taskTable').DataTable({
                "lengthChange": false,
                "searching": false,
                data: taskTableData,
                columns: [
                    { data: "taskId", title: "Task ID", width: "10%" },
                    { data: "taskName", title: "Task Name", width: "10%" },
                    { data: "assignedTo", title: "Assigned To", width: "10%" },
                    { data: "startDate", title: "Start Date", width: "10%" },
                    { data: "expiryDate", title: "End Date", width: "10%" },
                    { data: "status", title: "Status", width: "10%" },
                    { data: "actions", title: "Actions", width: "35%" }

                ],
                "columnDefs": [{
                    "targets": -1,
                    "data": null,
                    "defaultContent": ""
                }],
                "fnCreatedRow": function (nRow, aData, iDataIndex) {
                    var isTaskInProgress = (aData.status == "In Progress") ? true : false;
                    var isTaskCompleted = (aData.status == "Completed") ? true : false;
                    var isTaskApproved = (aData.status == "Approved") ? true : false;
                    $('td:eq(6)', nRow).append("<button class='button' id='view' style='border-radius: 5px;'>View</button><button class='button" + ($scope.isProjectManager && isTaskInProgress ? "" : " ng-hide") + "' id='edit' style='border-radius: 5px;'>Edit</button><button class='button" + ($scope.isProjectManager && isTaskInProgress ? "" : " ng-hide") + "' id='cancel' style='border-radius: 5px;'>Cancel</button><button class='button" + (!$scope.isProjectManager && isTaskInProgress ? "" : " ng-hide") + "' id='done' style='border-radius: 5px;' >Done</button><button class='button" + ($scope.isProjectManager && isTaskCompleted ? "" : " ng-hide") + "' id='approve' style='border-radius: 5px;'>Approve</button><button class='button" + ($scope.isProjectManager && isTaskCompleted ? "" : " ng-hide") + "' id='reject' style='border-radius: 5px;'>Reject</button>");
                }
            });

            $('#taskTable tbody').on('click', "#view", function () {
                var data = $scope.taskTable.row($(this).parents('tr')).data();

                $scope.viewTaskDetail(getTaskDetailByTaskId(data.taskId, $scope.taskList));

                //$scope.currentTask = {
                //    taskId: currentTask.taskId,
                //    taskName: currentTask.taskName,
                //    taskDesc: currentTask.taskDesc,
                //    startDate: currentTask.startDate,
                //    expiryDate: currentTask.expiryDate,
                //    createdDate: currentTask.createdDate,
                //    createdBy: currentTask.createdBy,
                //    status: currentTask.status,
                //    assignedTo: cur
                //}

            });

            $('#taskTable tbody').on('click', "#edit", function () {
                var data = $scope.taskTable.row($(this).parents('tr')).data();

                $scope.editTaskPopup(getTaskDetailByTaskId(data.taskId, $scope.taskList));
                //alert('Hi Edit');
            });
            $('#taskTable tbody').on('click', "#cancel", function () {
                var data = $scope.taskTable.row($(this).parents('tr')).data();

                $scope.updateTaskStatus(data.taskId, "Cancelled");

                //  alert('Hi Cancel');
            });
            $('#taskTable tbody').on('click', "#done", function () {
                var data = $scope.taskTable.row($(this).parents('tr')).data();
                //  $('button#done').confirmation("toggle");

                $scope.updateTaskStatus(data.taskId, "Completed");

                // alert('Hi Done');
            });
            $('#taskTable tbody').on('click', "#approve", function () {
                var data = $scope.taskTable.row($(this).parents('tr')).data();
                $scope.updateTaskStatus(data.taskId, "Approved");
                // alert('Hi Approve');
            });
            $('#taskTable tbody').on('click', "#reject", function () {
                var data = $scope.taskTable.row($(this).parents('tr')).data();
                $scope.updateTaskStatus(data.taskId, "In Progress");
                // alert('Hi Reject');
            });


        }

        $scope.loadLeaveTable = function (leaveTableData) {

            $scope.leaveTable = $('#leaveTable').DataTable({
                "lengthChange": false,
                "searching": false,
                data: leaveTableData,
                columns: [
                    { data: "leaveId", title: "Leave ID", width: "10%" },
                    { data: "appliedBy", title: "Applied By", width: "10%" },
                    { data: "fromDate", title: "Start Date", width: "10%" },
                    { data: "toDate", title: "End Date", width: "10%" },
                    { data: "leaveType", title: "Leave Type", width: "10%" },
                    { data: "status", title: "Status", width: "10%" },
                    { data: "actions", title: "Actions", width: "35%" }

                ],
                "columnDefs": [{
                    "targets": -1,
                    "data": null,
                    "defaultContent": ""
                }],
                "fnCreatedRow": function (nRow, aData, iDataIndex) {
                    var isLeaveApprovalPending = (aData.status == "Approval Pending") ? true : false;
                    var isLeaveCancelled = (aData.status == "Cancelled") ? true : false;
                    var isLeaveApproved = (aData.status == "Approved") ? true : false;
                    $('td:eq(6)', nRow).append("<button class='button' id='view' style='border-radius: 5px;'>View</button><button class='button" + (!$scope.isProjectManager && isLeaveApprovalPending ? "" : " ng-hide") + "' id='cancel' style='border-radius: 5px;'>Cancel</button><button class='button" + ($scope.isProjectManager && isLeaveApprovalPending ? "" : " ng-hide") + "' id='approve' style='border-radius: 5px;'>Approve</button><button class='button" + ($scope.isProjectManager && isLeaveApprovalPending ? "" : " ng-hide") + "' id='reject' style='border-radius: 5px;'>Reject</button>");
                }
            });

            $('#leaveTable tbody').on('click', "#view", function () {
                var data = $scope.leaveTable.row($(this).parents('tr')).data();
                $scope.currentLeave = getLeaveDetailByLeaveId(data.leaveId, $scope.leaveList);
                $scope.$apply();
                $timeout(function () {
                    $('#leaveDetailModal').modal("show")
                });



                //   alert('Hi View');
            });

            $('#leaveTable tbody').on('click', "#cancel", function () {
                var data = $scope.leaveTable.row($(this).parents('tr')).data();

                $scope.updateLeaveStatus(data.leaveId, "Cancelled");

                //  alert('Hi Cancel');
            });

            $('#leaveTable tbody').on('click', "#approve", function () {
                var data = $scope.leaveTable.row($(this).parents('tr')).data();
                $scope.updateLeaveStatus(data.leaveId, "Approved");
                // alert('Hi Approve');
            });
            $('#leaveTable tbody').on('click', "#reject", function () {
                var data = $scope.leaveTable.row($(this).parents('tr')).data();
                $scope.updateLeaveStatus(data.leaveId, "Cancelled");
                // alert('Hi Reject');
            });


        }


        $scope.loadTable = function (taskTableData) {

            $scope.loadTaskTable(taskTableData);
            $scope.loadLeaveTable(taskTableData);


        }

        // Function - Home Page Initialization
        $scope.homeInit = function () {

            var ntid = sessionStorage.getItem("username");
            if (ntid != null || ntid != undefined) {

                $('.loader').show();
                $('.backdrop').show();

                var postData = { ntid: ntid };

                //Ajax method 
                $http({
                    method: "POST",
                    url: "/TaskManagerAPI.aspx/ShowData",
                    data: JSON.stringify(postData),
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true

                }).then(function mySuccess(response) {

                    var responseJSON = JSON.parse(response.data.d);

                    $scope.status = responseJSON.Response.Status;

                    $('.loader').hide();
                    $('.backdrop').hide();

                    if ($scope.status == "Success") {

                        $scope.user = JSON.parse(responseJSON.Response.userObject);
                        if ($scope.user.roleId == "201")
                            $scope.isProjectManager = true;
                        $scope.taskList = JSON.parse(responseJSON.Response.taskObject);
                        $scope.leaveList = JSON.parse(responseJSON.Response.leaveObject);

                        $scope.loadTaskTable($scope.taskList);
                        $scope.loadLeaveTable($scope.leaveList);

                        $scope.initializeConfirmationDialog();

                    }
                }, function myError(response) {
                    console.log(response);
                });

            } else {
                window.location.href = "]";
            }

        }



        var self = this;



        $scope.refreshTaskTable = function (tableData) {
            if ($scope.taskTable != null || $scope.taskTable != undefined) {
                $scope.taskTable.clear();
                $scope.taskTable.rows.add(tableData);
                $scope.taskTable.draw();
            } else {
                $scope.loadTaskTable(tableData);
            }
            $scope.initializeConfirmationDialog();
        }

        $scope.refreshLeaveTable = function (tableData) {
            if ($scope.leaveTable != null || $scope.leaveTable != undefined) {
                $scope.leaveTable.clear();
                $scope.leaveTable.rows.add(tableData);
                $scope.leaveTable.draw();
            } else {
                $scope.loadLeaveTable(tableData);
            }
            $scope.initializeConfirmationDialog();
        }



        /******************************** TABLE BUTTONS CODE *********************************/

        // Fuction - To initialize confirmation dialog on Table Action Buttons

        $scope.initializeConfirmationDialog = function () {

            var tableButtonArray = ["done", "cancel", "approve", "reject"];

            for (var but in tableButtonArray) {
                $('button#' + tableButtonArray[but]).confirmation({
                    rootSelector: 'button#' + tableButtonArray[but],
                    // other options
                    onConfirm: function () {
                    },
                    onCancel: function () {
                    }
                });
            }
        }



        /******************************** LOGOUT CODE *********************************/

        // Function - Logout
        $scope.logout = function () {
            sessionStorage.removeItem("username");
            window.location.href = "signin";
        }

        /******************************** CREATE NEW TASK CODE *********************************/


        // Task Handler - Decides whether to create a new task or to update a task
        $scope.taskHandler = function () {
            if ($scope.taskModalTitle == "Create New Task")
                $scope.createTask();

            else if ($scope.taskModalTitle == "Edit Task Detail")
                $scope.editTask();
        }


        // Display Popup - Create Task
        $scope.createTaskPopup = function () {

            //Ajax method 
            $http({
                method: "POST",
                url: "/TaskManagerAPI.aspx/GetUserList",
                data: "",
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true

            }).then(function mySuccess(response) {

                var responseJSON = JSON.parse(response.data.d);

                $scope.status = responseJSON.Response.Status;


                if ($scope.status == "Success") {
                    usersJSON = JSON.parse(responseJSON.Response.userObject);
                    $scope.availableTeamMembers = parseTeamMembers(usersJSON);

                    $scope.taskModalTitle = "Create New Task";
                    $scope.task = {};

                    taskMinStartDate = moment().format('MM/DD/YYYY');

                    $('#taskModal').modal("show");

                    $timeout(function () {
                        $('#taskForm').data('formValidation').resetForm();
                    }, 200);


                }
            }, function myError(response) {
                console.log(response);
            });


        }

        // Form Validation - Task Form 
        $('#taskForm').formValidation({
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
                            message: 'Please enter task name'
                        }
                    }
                }, taskDesc: {
                    validators: {
                        stringLength: {
                            min: 5,
                            max: 150
                        },
                        notEmpty: {
                            message: 'Please enter task description'
                        }
                    }
                },
                taskStartDate: {
                    validators: {
                        stringLength: {
                            min: 10,
                            max: 10,
                        },
                        date: {

                            format: 'MM/DD/YYYY',
                            message: 'Please select a valid date'

                        },
                        callback: {
                            message: 'The date is not in the range',
                            callback: function (value, validator) {
                                var m = new moment(value, 'MM/DD/YYYY', true);
                                if (!m.isValid()) {
                                    return false;
                                }
                                $scope.task.taskStartDate = value;
                                return m.isSameOrAfter(moment(taskMinStartDate));
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
                            min: 10,
                            max: 10,
                        },
                        date: {

                            format: 'MM/DD/YYYY',
                            message: 'Please enter a valid date'

                        },
                        callback: {
                            message: 'The date is not in the range',
                            callback: function (value, validator) {
                                var m = new moment(value, 'MM/DD/YYYY', true);
                                if (!m.isValid()) {
                                    return false;
                                }
                                $scope.task.taskEndDate = value;
                                return m.isSameOrAfter(moment($scope.task.taskStartDate));
                            }
                        },

                        notEmpty: {
                            message: 'Please select end date'
                        }
                    }
                },
                assignedTo: {
                    stringLength: {
                        min: 5,

                    },
                    validators: {
                        notEmpty: {
                            message: 'Please select an associate to assign task'
                        }
                    }
                }
            }
        });

        // Function - Create Task
        $scope.createTask = function () {


            $('#taskForm').data('formValidation').validate();

            if ($('#taskForm').data('formValidation').isValid() != null && $('#taskForm').data('formValidation').isValid()) {

                $('.loader').show();
                $('.backdrop').show();

                var postData = {

                    taskDesc: $scope.task.taskDesc,
                    expiryDate: $scope.task.taskEndDate,
                    createdBy: sessionStorage.getItem("username"),
                    assignedTo: $scope.task.assignedTo,
                    status: "In Progress",
                    taskName: $scope.task.taskName,
                    startDate: $scope.task.taskStartDate

                }

                //Ajax method 

                $http({
                    method: "POST",
                    url: "/TaskManagerAPI.aspx/CreateTask",
                    data: JSON.stringify(postData),
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true
                }).then(function mySuccess(response) {

                    $scope.task = {};

                    $('#taskModal').modal("hide");

                    var responseJSON = JSON.parse(response.data.d);
                    $scope.status = responseJSON.Response.Status;
                    $scope.reason = responseJSON.Response.Reason;

                    $('.loader').hide();
                    $('.backdrop').hide();

                    if ($scope.status == "Success") {

                        //$scope.statusModalTitle = "Task ";

                        $scope.taskList = JSON.parse(responseJSON.Response.taskObject);
                        $scope.refreshTaskTable($scope.taskList);

                        $('#statusModal').modal('show');

                    }
                    else if ($scope.status == "Failure")
                        $('#statusModal').modal('show');

                }, function myError(response) {

                    console.log(response);

                });
            }

        }

        /******************************** EDIT TASK DETAIL CODE *********************************/


        $scope.editTaskPopup = function (taskData) {

            //Ajax method 
            $http({
                method: "POST",
                url: "/TaskManagerAPI.aspx/GetUserList",
                data: "",
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true

            }).then(function mySuccess(response) {

                var responseJSON = JSON.parse(response.data.d);

                $scope.status = responseJSON.Response.Status;

                if ($scope.status == "Success") {
                    usersJSON = JSON.parse(responseJSON.Response.userObject);
                    $scope.availableTeamMembers = parseTeamMembers(usersJSON);
                    $scope.taskModalTitle = "Edit Task Detail";

                    $scope.task = {
                        taskId: taskData.taskId,
                        taskName: taskData.taskName,
                        taskDesc: taskData.taskDesc,
                        taskStartDate: moment(taskData.startDate).format('MM/DD/YYYY'),
                        taskEndDate: moment(taskData.expiryDate).format('MM/DD/YYYY'),
                        assignedTo: taskData.assignedTo,
                        status: taskData.status
                    }

                    taskMinStartDate = $scope.task.taskStartDate;

                    $('#taskModal').modal("show");

                    $timeout(function () {
                        $('#taskForm').data('formValidation').resetForm();
                    }, 200);

                }
            }, function myError(response) {
                console.log(response);
            });



        }


        $scope.editTask = function () {

            $('#taskForm').data('formValidation').validate();

            if ($('#taskForm').data('formValidation').isValid() != null && $('#taskForm').data('formValidation').isValid()) {

                $('.loader').show();
                $('.backdrop').show();

                var postData = {
                    ntid: sessionStorage.getItem('username'),
                    taskId: $scope.task.taskId,
                    taskDesc: $scope.task.taskDesc,
                    expiryDate: $scope.task.taskEndDate,
                    assignedTo: $scope.task.assignedTo,
                    taskName: $scope.task.taskName,
                    status: null
                }

                //Ajax method 

                $http({
                    method: "POST",
                    url: "/TaskManagerAPI.aspx/UpdateTask",
                    data: JSON.stringify(postData),
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true
                }).then(function mySuccess(response) {



                    $('#taskModal').modal("hide");

                    var responseJSON = JSON.parse(response.data.d);
                    $scope.status = responseJSON.Response.Status;
                    $scope.reason = responseJSON.Response.Reason;

                    $('.loader').hide();
                    $('.backdrop').hide();

                    if ($scope.status == "Success") {

                        //$scope.taskList = updateTaskDetails($scope.task, $scope.taskList);
                        //$scope.refreshTable($scope.taskList);

                        var taskJSON = JSON.parse(responseJSON.Response.taskObject);
                        $scope.refreshTaskTable(taskJSON);

                        $scope.task = {};
                        $('#statusModal').modal('show');

                    }
                    else if ($scope.status == "Failure")
                        $('#statusModal').modal('show');

                }, function myError(response) {

                    console.log(response);

                });



            }


        }



        /******************************** VIEW TASK DETAIL CODE *********************************/


        $scope.viewTaskDetail = function (taskDetailData) {

            $scope.currentTask = taskDetailData;
            $scope.$apply();
            $timeout(function () {
                $('#taskDetailModal').modal("show");
            });




        }


        /******************************** UPDATE TASK STATUS CODE *********************************/

        $scope.updateTaskStatus = function (taskId, status) {
            $('.loader').show();
            $('.backdrop').show();

            var postData = {
                ntid: sessionStorage.getItem('username'),
                taskId: taskId,
                taskDesc: null,
                expiryDate: null,
                assignedTo: null,
                taskName: null,
                status: status
            }

            $http({
                method: "POST",
                url: "/TaskManagerAPI.aspx/UpdateTask",
                data: JSON.stringify(postData),
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true
            }).then(function mySuccess(response) {

                var responseJSON = JSON.parse(response.data.d);
                $scope.status = responseJSON.Response.Status;
                $scope.reason = responseJSON.Response.Reason;

                $('.loader').hide();
                $('.backdrop').hide();

                if ($scope.status == "Success") {

                    $scope.taskList = JSON.parse(responseJSON.Response.taskObject);
                    $scope.refreshTaskTable($scope.taskList);

                    $('#statusModal').modal('show');

                }
                else if ($scope.status == "Failure")
                    $('#statusModal').modal('show');

            }, function myError(response) {

                console.log(response);

            });



        }


        /******************************** APPLY LEAVE CODE *********************************/

        // Function - Display Leave Popup
        $scope.applyLeavePopup = function () {

            $scope.leaveModalTitle = "Apply Leave";
            $scope.leave = {};

            leaveMinStartDate = moment().format('MM/DD/YYYY');

            $('#leaveModal').modal("show");

            $timeout(function () {
                $('#leaveForm').data('formValidation').resetForm();
            }, 200);

        }

        // Form Validation - Leave Form 
        $('#leaveForm').formValidation({
            // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                leaveDesc: {
                    validators: {
                        stringLength: {
                            min: 5,
                            max: 150
                        },
                        notEmpty: {
                            message: 'Please enter detailed reason for leave'
                        }
                    }
                },
                leaveStartDate: {
                    validators: {
                        stringLength: {
                            min: 10,
                            max: 10,
                        },
                        date: {

                            format: 'MM/DD/YYYY',
                            message: 'Please select a valid date'

                        },
                        callback: {
                            message: 'The date is not in the range',
                            callback: function (value, validator) {
                                var m = new moment(value, 'MM/DD/YYYY', true);
                                if (!m.isValid()) {
                                    return false;
                                }
                                $scope.leave.leaveStartDate = value;
                                return m.isSameOrAfter(moment(leaveMinStartDate));
                            }
                        },
                        notEmpty: {
                            message: 'Please select start date'
                        }
                    }
                },
                leaveEndDate: {
                    validators: {
                        stringLength: {
                            min: 10,
                            max: 10,
                        },
                        date: {

                            format: 'MM/DD/YYYY',
                            message: 'Please select a valid date'

                        },
                        callback: {
                            message: 'The date is not in the range',
                            callback: function (value, validator) {
                                var m = new moment(value, 'MM/DD/YYYY', true);
                                if (!m.isValid()) {
                                    return false;
                                }
                                $scope.leave.leaveEndDate = value;
                                return m.isSameOrAfter(moment($scope.leave.leaveStartDate));
                            }
                        },

                        notEmpty: {
                            message: 'Please select end date'
                        }
                    }
                },
                leaveType: {
                    stringLength: {
                        min: 5,

                    },
                    validators: {
                        notEmpty: {
                            message: 'Please select leave type'
                        }
                    }
                }
            }
        });

        // Function - Apply Leave
        $scope.applyLeave = function () {

            $('#leaveForm').data('formValidation').validate();
            if ($('#leaveForm').data('formValidation').isValid() != null && $('#leaveForm').data('formValidation').isValid()) {

                $('.loader').show();
                $('.backdrop').show();

                var postData = {

                    fromDate: $scope.leave.leaveStartDate,
                    toDate: $scope.leave.leaveEndDate,
                    leaveType: $scope.leave.leaveType,
                    appliedBy: sessionStorage.getItem("username"),
                    leaveDesc: $scope.leave.leaveDesc,
                    status: "Approval Pending"

                }

                //Ajax method 

                $http({
                    method: "POST",
                    url: "/TaskManagerAPI.aspx/ApplyLeave",
                    data: JSON.stringify(postData),
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true
                }).then(function mySuccess(response) {

                    $scope.leave = {};

                    $('#leaveModal').modal("hide");

                    var responseJSON = JSON.parse(response.data.d);
                    $scope.status = responseJSON.Response.Status;
                    $scope.reason = responseJSON.Response.Reason;

                    $('.loader').hide();
                    $('.backdrop').hide();

                    if ($scope.status == "Success") {

                        $scope.leaveList = JSON.parse(responseJSON.Response.leaveObject);
                        $scope.refreshLeaveTable($scope.leaveList);

                        $('#statusModal').modal('show');

                    }
                    else if ($scope.status == "Failure")
                        $('#statusModal').modal('show');

                }, function myError(response) {

                    console.log(response);

                });
            }

        }
        /******************************** UPDATE LEAVE STATUS CODE *********************************/

        // Function - Update Leave Status

        $scope.updateLeaveStatus = function (leaveId, status) {

            $('.loader').show();
            $('.backdrop').show();

            var postData = {
                ntid: sessionStorage.getItem('username'),
                leaveId: leaveId,
                status: status
            }

            $http({
                method: "POST",
                url: "/TaskManagerAPI.aspx/UpdateLeaveDetails",
                data: JSON.stringify(postData),
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true
            }).then(function mySuccess(response) {

                var responseJSON = JSON.parse(response.data.d);
                $scope.status = responseJSON.Response.Status;
                $scope.reason = responseJSON.Response.Reason;

                $('.loader').hide();
                $('.backdrop').hide();

                if ($scope.status == "Success") {

                    $scope.leaveList = JSON.parse(responseJSON.Response.leaveObject);
                    $scope.refreshLeaveTable($scope.leaveList);
                    $('#statusModal').modal('show');

                }
                else if ($scope.status == "Failure")
                    $('#statusModal').modal('show');

            }, function myError(response) {

                console.log(response);

            });



        }





        /******************************** CHANGE PASSWORD CODE *********************************/


        // Display Popup - Change password 
        $scope.changePwdPopUp = function () {

            var options = {
                "backdrop": "static"
            }
            $('#changePwdModal').modal(options);
        }


        // Form Validation - Change Password
        $('#change_form').formValidation({
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
                            message: 'Please enter your current password'
                        }
                    }
                },
                newPwd: {
                    validators: {
                        stringLength: {
                            min: 5,
                        },
                        different: {
                            field: 'currentPwd',
                            message: 'New Password must be different from current password'
                        },
                        notEmpty: {
                            message: 'Please enter your new password'
                        }
                    }
                },
                confirmNewPwd: {
                    validators: {
                        stringLength: {
                            min: 5,
                        },
                        notEmpty: {
                            message: 'Please re-enter your new password'
                        },
                        identical: {
                            field: 'newPwd',
                            message: 'Passwords do not match. Please check!!!'
                        }
                    }
                }
            }
        });

        // Function - Change Password
        $scope.changePassword = function () {

            $('#change_form').data('formValidation').validate();

            if ($('#change_form').data('formValidation').isValid() != null && $('#change_form').data('formValidation').isValid()) {

                $('.loader').show();
                $('.backdrop').show();

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
                    async: true
                }).then(function mySuccess(response) {

                    var responseJSON = JSON.parse(response.data.d);
                    $scope.reason = responseJSON.Response.Reason;
                    $scope.status = responseJSON.Response.Status;

                    $('.loader').hide();
                    $('.backdrop').hide();

                    if ($scope.status == "Success") {

                    } else if ($scope.status == "Failure") {

                    }
                    $('#changePwdModal').modal("hide");


                    $('#statusModal').modal("show");

                }, function myError(response) {
                    console.log(response);
                });
            }
        }

        $scope.homeInit();

    });

});

//******************* Login Controller *******************//

app.controller('loginCtrl', function ($scope, $rootScope, $http, httpService, $interval, $cookies) {

    $scope.isDisableLoginButton = true;

    //Check weather user has already logged 
    $scope.pageInit = function () {
        if (!(sessionStorage.getItem('username') == null || sessionStorage.getItem('username') == undefined)) {
            goToURL('home');
        }
    }

    //$scope.signupInit = function () {

    //    if (!(sessionStorage.getItem('username') == null || sessionStorage.getItem('username') == undefined)) {
    //        goToURL('home');
    //    }
    //    else {
    //        var postData = {}
    //        //Ajax method - To Get User NTID
    //        $http({
    //            method: "POST",
    //            url: "/TaskManagerAPI.aspx/GetUserDetails",
    //            data: JSON.stringify(postData),
    //            cache: false,
    //            contentType: "application/json; charset=utf-8",
    //            dataType: "json",
    //            async: true
    //        }).then(function mySucces(response) {

    //            var responseJSON = JSON.parse(response.data.d);

    //            $scope.status = responseJSON.Response.Status;

    //            if ($scope.status == "Success") {

    //                var userJSON = JSON.parse(responseJSON.Response.userObject);
    //                $scope.ntid_name = userJSON.ntid;

    //            }

    //        }, function myError(response) {
    //            console.log(response);
    //        });
    //    }



    //}


    $(function () {

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
        $('#login_form').formValidation({
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
                            message: 'Please enter your NTID'
                        }
                    }
                },
                password: {
                    validators: {
                        stringLength: {
                            min: 5,
                        },
                        notEmpty: {
                            message: 'Please enter your password'
                        }
                    }
                }
            }
        }).on('success.field.fv', function (e, data) {

            if ($('#login_form').data('formValidation').isValid() != null && $('#login_form').data('formValidation').isValid()) {
                $scope.isDisableLoginButton = false;
            }
            else
                $scope.isDisableLoginButton = true;

            //if (data.fv.getInvalidFields().length > 0) {    // There is invalid field
            //    //data.fv.disableSubmitButtons(true);

            //}
            //else
            //    $('#signInButton').disable(false);
        }).on('err.field.fv', function (e, data) {
            if ($('#login_form').data('formValidation').isValid() != null && $('#login_form').data('formValidation').isValid()) {
                $scope.isDisableLoginButton = false;
            }
            else
                $scope.isDisableLoginButton = true;

        });

        // Function - Sign-In 

        $scope.login = function () {

            $('#login_form').data('formValidation').validate();

            if ($('#login_form').data('formValidation').isValid() != null && $('#login_form').data('formValidation').isValid()) {

                $('.loader').show();
                $('.backdrop').show();

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
                    async: true
                }).then(function mySuccess(response) {

                    var responseJSON = JSON.parse(response.data.d);

                    $scope.status = responseJSON.Response.Status;
                    $scope.reason = responseJSON.Response.Reason;

                    $('.loader').hide();
                    $('.backdrop').hide();

                    if ($scope.status == "Success") {
                        //Save NTID in session storage
                        sessionStorage.setItem('username', $scope.ntid);
                        window.location.href = "home.aspx";

                    }
                    else if ($scope.status == "Failure") {

                        $("#statusModal").modal("show");

                    }
                }, function myError(response) {
                    console.log(response);
                });


            }
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
        $('#forgot_form').formValidation({
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
        });

        // Function - Forgot Password 
        $scope.sendPwd = function () {

            $('#forgot_form').data('formValidation').validate();

            if ($('#forgot_form').data('formValidation').isValid() != null && $('#forgot_form').data('formValidation').isValid()) {

                $('.loader').show();
                $('.backdrop').show();

                var user = {
                    ntid: $scope.forgot_ntid,
                }

                //Ajax method 
                $http({
                    method: "POST",
                    url: "/TaskManagerAPI.aspx/ForgotPassword",
                    data: JSON.stringify(user),
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true
                }).then(function mySuccess(response) {

                    var responseJSON = JSON.parse(response.data.d);

                    $scope.status = responseJSON.Response.Status;
                    $scope.reason = responseJSON.Response.Reason;

                    $('.loader').hide();
                    $('.backdrop').hide();

                    $('#forgotPwd').modal("hide");

                    $('#statusModal').modal("show");

                }, function myError(response) {
                    console.log(response);
                });

                console.log(user.toString());
            }
        }

        /**************************** SIGN-UP CODE ***************************************/

        $('#register_form').formValidation({

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
                            message: 'Please enter your first name.'
                        }
                    }
                }, last_name: {
                    validators: {
                        stringLength: {
                            min: 2,
                        },
                        notEmpty: {
                            message: 'Please enter your last name.'
                        }
                    }
                },


                role_id: {
                    validators: {
                        notEmpty: {
                            message: 'Please select your role'
                        }
                    }
                },
                ntid_name: {
                    validators: {
                        stringLength: {
                            min: 2,
                        },
                        notEmpty: {
                            message: 'Please enter your NTID'
                        }
                    }
                },
                password: {
                    validators: {
                        stringLength: {
                            min: 5,
                        },
                        notEmpty: {
                            message: 'Please enter your password (Min 5 characters)'
                        }
                    }
                },
                confpassword: {
                    validators: {
                        stringLength: {
                            min: 5,
                        },
                        notEmpty: {
                            message: 'Please re-enter your password'
                        },
                        identical: {
                            field: 'password',
                            message: 'Passwords do not match. Please check!!!'
                        }
                    }
                },
                email: {
                    validators: {
                        notEmpty: {
                            message: 'Please enter your email address'
                        },
                        emailAddress: {
                            message: 'Please enter a valid email address'
                        },
                        regexp: {
                            regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                            message: 'The value is not a valid email address'
                        }
                    }
                },
                phone: {
                    validators: {
                        stringLength: {
                            min: 10,
                            max: 10
                        },
                        notEmpty: {
                            message: 'Please enter your phone number'
                        },
                        phone: {
                            country: 'IN',
                            message: 'Please enter a vaild phone number'
                        }
                    }
                }
            }
        });

        // Function - Sign-Up 
        $scope.createAccount = function () {

            $('#register_form').data('formValidation').validate();

            if ($('#register_form').data('formValidation').isValid() != null && $('#register_form').data('formValidation').isValid()) {

                $('.loader').show();
                $('.backdrop').show();

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
                    async: true
                }).then(function mySuccess(response) {

                    var resp = JSON.parse(response.data.d);

                    $('.loader').hide();
                    $('.backdrop').hide();

                    //Case of NTID already existing
                    if (resp.Response.Status == 'Failure') {
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
                    $('#statusModal').modal(options);

                    //Redirect to Sign In page after a specific time interval 
                    //setTimeout(function () {
                    //    window.location.href = "SignIn.aspx";
                    //}, 5000);
                }), function myError(response) {
                    console.log(response);
                }

            }




        }

    });

});// Login Controller Ends Here


/******************************** UTILITY FUNCTIONS *********************************/

//Navigate to Page
function goToURL(navigatePage) {
    if (navigatePage != null && navigatePage != '')
        window.location.href = navigatePage;
}

// Function - Task List Parsing
function parseTaskList(taskList) {

    var refinedTaskList = [];

    for (i = 0; i < taskList.length; i++) {
        var taskArr = [];
        taskArr.push(taskList[i].taskId.toString());
        taskArr.push(taskList[i].taskName);
        taskArr.push(taskList[i].assignedTo);
        taskArr.push(taskList[i].startDate);
        taskArr.push(taskList[i].expiryDate);
        taskArr.push(taskList[i].status);

        refinedTaskList.push(taskArr);
    }

    return refinedTaskList;

}

// Function - Team Members List Parsing
function parseTeamMembers(tmList) {
    var teamMembersList = [];
    for (i = 0; i < tmList.length; i++) {
        var tm = {};
        tm.name = tmList[i].name;
        tm.ntid = tmList[i].ntid;
        teamMembersList.push(tm);
    }
    return teamMembersList;
}

// Function - Create Button for Task Table
function createTaskButton(name, classname, clickHandler) {

    var button = document.createElement("button");
    button.setAttribute("name", name);
    button.setAttribute("class", classname);
    button.setAttribute("id", name);
    //   button.setAttribute("ng-click", clickHandler + "()");
    button.innerText = name;
    button.style.borderRadius = "5px";
    return button;
}

// Function - Get Task Detail By Task Id
function getTaskDetailByTaskId(taskId, taskList) {

    var taskDetail = {};

    for (i = 0; i < taskList.length; i++) {
        if (taskList[i].taskId == taskId) {
            taskDetail = taskList[i];
            break;
        }
    }
    return taskDetail;
}

// Function - Get Leave Detail By Leave Id
function getLeaveDetailByLeaveId(leaveId, leaveList) {

    var leaveDetail = {};

    for (i = 0; i < leaveList.length; i++) {
        if (leaveList[i].leaveId == leaveId) {
            leaveDetail = leaveList[i];
            break;
        }
    }
    return leaveDetail;
}


function User(userData) {
    this.firstName = userData.firstName;
    this.lastName = userData.lastName;

}


function updateTaskDetails(task, taskList) {

    for (i = 0; i < taskList.length; i++) {
        if (taskList[i].taskId == task.taskId) {
            taskList[i].taskName = task.taskName;
            taskList[i].taskDesc = task.taskDesc;
            taskList[i].expiryDate = task.taskEndDate;
            taskList[i].assignedTo = task.assignedTo;
            break;
        }
    }
    return taskList;
}
