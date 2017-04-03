<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="taskPage.aspx.cs" Inherits="Task_and_Leave_Tracker.Index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <link rel="import" href="header.html">
    <title>Task & Leave Tracker Tool</title>
</head>
<body ng-app="myApp" ng-controller="homeCtrl" style="background-image: url(images/background_pattern.jpg);" ng-init="refreshTable()">

    <div class="container type-wrap" style="height: auto">

        <div header></div>
        <!--Form section-->
        <div class="row">
            <div class="col-6">
                <i class="fa fa-plus-circle"></i>
                <button style="display: inline" type="button" class="button">Create Task</button>
                <button style="float: right; display: inline; margin-right: 20px" type="button" class="button" data-toggle="modal" data-target="#changePwdModal">Change Password</button>

            </div>


            <div class="col-6">
                <p style="margin-top: 20px">{{time}}</p>

            </div>
        </div>

        <!-- Data Table -->
        <div class="row" style="margin-top: 20px">
            <div class="col-2"></div>

            <div class="col-8">

                <table id="myTable" style="width: 730px">
                    <thead>
                        <tr>
                            <!--Defined in code-->

                        </tr>
                    </thead>
                </table>



            </div>
            <div class="col-2"></div>

        </div>

        <!--Bottom most section-->
        <div class="row" style="margin-top: 20px">

            <div class="col-6">
                <!--email sending-->
                <button type="button" class="button">Email Pending Tasks</button>
            </div>
            <div class="col-6">

                <!--Mark All completion-->
                <button type="button" class="button" style="float: right;">Mark All Completed</button>
            </div>
        </div>

    </div>
     <!--Change Password-->
    <div class="modal fade" id="changePwdModal" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">

                    <h4 class="modal-title" id="myModalLabel">Change Password</h4>
                </div>

                <div class="modal-body">

                    <form id="change_form" class="form-horizontal">
                        <!--Current Password-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Current Password</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="currentPwd" name="currentPwd" placeholder="**********" class="form-control" type="password" ng-model="currentPwd" />
                                </div>
                            </div>
                        </div>

                        <!-- New Password-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">New Password</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="newPwd" name="newPwd" placeholder="**********" class="form-control" type="password" ng-model="newPwd" />
                                </div>
                            </div>
                        </div>


                        <!-- Confirm New Password field-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Confirm New Password</label>
                            <div class="col-md-4 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="confirmNewPwd" name="confirmNewPwd" placeholder="**********" class="form-control" type="password" ng-model="confirmNewPwd" />
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <a href="#" class="btn btn-default button" style="padding-top: 16px" data-dismiss="modal" ng-click="changePassword()">Send</a>
                    <a href="#" class="btn btn-default button" style="padding-top: 16px" data-dismiss="modal">Close</a>

                </div>

            </div>
        </div>
    </div>
</body>
</html>

