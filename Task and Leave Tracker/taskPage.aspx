<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="taskPage.aspx.cs" Inherits="Task_and_Leave_Tracker.Index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <link rel="import" href="header.html">
    <title>Task & Leave Tracker Tool</title>
</head>
<body ng-app="myApp" ng-controller="homeCtrl" style="background-image: url(images/background_pattern.jpg);" ng-init="homeInit()">

    <div class="container type-wrap" style="height: auto">

        <div header></div>
        <!--Form section-->
        <div class="row">
            <div class="col-6">
                <i class="fa fa-plus-circle"></i>
                <button ng-show="isProjectManager" style="display: inline" type="button" class="button" ng-click="createTaskPopup()">Create Task</button>
                <button style="float: right; display: inline; margin-right: 20px" type="button" class="button" data-toggle="modal" data-target="#changePwdModal">Change Password</button>

            </div>


            <div class="col-6">
                <p style="margin-top: 20px">{{time}}</p>

            </div>
        </div>

        <!-- Data Table -->
        <div class="row" style="margin-top: 20px">
            <div class="col-1"></div>

            <div class="col-10">

                <table id="myTable" style="margin: 0">
                    <thead>
                        <tr>
                            <!--Defined in code-->

                        </tr>
                    </thead>
                </table>
            </div>
            <div class="col-1"></div>

        </div>

        <!--Bottom most section-->
        <div class="row" style="margin-top: 20px; margin-bottom: 20px">

            <!--email sending-->
            <button style="display: inline" type="button" class="button">Email Pending Tasks</button>

            <!--Mark All completion-->
            <button style="float: right; display: inline; margin-right: 20px" type="button" class="button" style="float: right;">Mark All Completed</button>


        </div>

    </div>

    <!-- Change Password Modal -->
    <div class="modal fade" id="changePwdModal" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">

                    <h4 class="modal-title" id="myModalLabel">Change Password</h4>
                </div>
                <form id="change_form" class="form-horizontal">
                    <div class="modal-body">


                        <!--Current Password-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Current Password</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="currentPwd" name="currentPwd" placeholder="**********" class="form-control" type="password" ng-model="currentPwd" />
                                </div>
                            </div>
                        </div>

                        <!-- New Password-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">New Password</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="newPwd" name="newPwd" placeholder="**********" class="form-control" type="password" ng-model="newPwd" />
                                </div>
                            </div>
                        </div>


                        <!-- Confirm New Password field-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Confirm New Password</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="confirmNewPwd" name="confirmNewPwd" placeholder="**********" class="form-control" type="password" ng-model="confirmNewPwd" />
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="modal-footer">
                        <button class="button" ng-click="changePassword()">Submit</button>
                        <button class="button" data-dismiss="modal">Cancel</button>

                    </div>
                </form>
            </div>
        </div>
    </div>

    <!-- Create Task Modal -->
    <div class="modal fade" id="createTaskModal" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">

                    <h4 style="text-align: center" class="modal-title" id="createTaskTitle">Create New Task</h4>
                </div>

                <div class="modal-body">

                    <form id="taskForm" class="form-horizontal">
                        <!--Task Name-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Task Name</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="taskName" name="taskName" placeholder="Enter Task Name" class="form-control" type="text" ng-model="taskName" />
                                </div>
                            </div>
                        </div>

                        <!-- Task Description-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Task Description</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <textarea id="taskDesc" name="taskDesc" rows="2" cols="40" placeholder="Enter Task Description" class="form-control" ng-model="taskDesc"> </textarea>

                                </div>
                            </div>
                        </div>


                        <!-- Task Start Date-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Task Start Date</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="taskStartDate" name="taskStartDate" class="form-control" type="date" ng-model="taskStartDate" />
                                </div>
                            </div>
                        </div>

                        <!-- Task End Date-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Task End Date</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="taskEndDate" name="taskEndDate" class="form-control" type="date" ng-model="taskEndDate" />
                                </div>
                            </div>
                        </div>

                        <!-- Assigned To-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Assigned To</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <select title="Select Associate" id="assignedTo" name="assignedTo" class="form-control" ng-model="assignedTo">
                                        <option ng-repeat="tm in availableTeamMembers track by $index" value="{{tm.ntid}}">{{tm.name}}</option>
                                    </select>

                                </div>
                            </div>
                        </div>

                    </form>
                </div>
                <div class="modal-footer">
                    <a href="#" class="btn btn-default button" id="createTaskButton" style="padding-top: 16px; width: auto" ng-click="createTask()">Create Task</a>
                    <a href="#" class="btn btn-default button" style="padding-top: 16px" data-dismiss="modal">Cancel</a>

                </div>

            </div>
        </div>
    </div>

    <!-- Status Notification Modal -->
    <div class="modal fade" id="statusModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalTitle" style="text-align: center">Create Task - <span ng-bind="status"></span></h4>
                </div>
                <div class="modal-body">
                    <h3 style="text-align: center" ng-bind="reason"></h3>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">OK</button>
                </div>
            </div>
        </div>
    </div>

</body>
</html>

