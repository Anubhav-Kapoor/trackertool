<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Task_and_Leave_Tracker.Index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <link rel="import" href="header.html">
    <title>Task & Leave Tracker Tool</title>
</head>
<body ng-app="myApp" ng-controller="homeCtrl" style="background-image: url(images/background_pattern.jpg);" ng-init="pageInit()">

    <div class="container type-wrap" style="height: auto">

        <div header></div>
        <!--Form section-->

        <div class="row">
            <div class="col-6">
                <i class="fa fa-plus-circle"></i>
                <button ng-show="isProjectManager" style="display: inline" type="button" class="button" ng-click="createTaskPopup()">Create Task</button>
                <button ng-show="!(isProjectManager)" style="display: inline" type="button" class="button" ng-click="applyLeavePopup()">Apply Leave</button>
                <button style="float: right; display: inline; margin-right: 20px" type="button" class="button" ng-click="logout()">Logout</button>
                <button style="float: right; display: inline; margin-right: 20px" type="button" class="button" ng-click="changePwdPopUp()">Change Password</button>

            </div>


            <div class="col-6">
                <p style="margin-top: 20px">{{time}}</p>

            </div>
        </div>

        <!-- Data Table -->
        <div class="row" style="margin-top: 20px">
            <div class="col-1"></div>

            <div class="col-10">

                 <ul class="nav nav-tabs">
                    <li class="active"><a data-toggle="tab" href="#tasks">Tasks</a></li>
                    <li><a data-toggle="tab" href="#leaves">Leaves</a></li>
                </ul>
                 <div class="tab-content">
                    <div id="tasks" class="tab-pane fade in active">
                        <table id="taskTable" style="margin: 0;width:100%">
                            <thead>
                                <tr>
                                    <!--Defined in code-->

                                </tr>
                            </thead>
                        </table>
                    </div>
                    <div id="leaves" class="tab-pane fade">
                        <table id="leaveTable" style="margin: 0;width:100%">
                            <thead>
                                <tr>
                                    <!--Defined in code-->

                                </tr>
                            </thead>
                        </table>
                    </div>
    
                </div>


                
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
    <div class="modal fade" id="changePwdModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">

                    <h4 class="modal-title" id="myModalLabel" style="text-align: center">Change Password</h4>
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
    <div class="modal fade" id="taskModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">

                    <h4 style="text-align: center" class="modal-title" ng-bind="taskModalTitle"></h4>
                </div>
                <form id="taskForm" class="form-horizontal">
                    <div class="modal-body">


                        <!--Task Name-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Task Name</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="taskName" name="taskName" placeholder="Enter Task Name" class="form-control" type="text" ng-model="task.taskName" />
                                </div>
                            </div>
                        </div>

                        <!-- Task Description-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Task Description</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <textarea id="taskDesc" name="taskDesc" rows="2" cols="40" placeholder="Enter Task Description" class="form-control" ng-model="task.taskDesc"> </textarea>

                                </div>
                            </div>
                        </div>


                        <!-- Task Start Date-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Task Start Date</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group date" id="taskStartDateField">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                    <input id="taskStartDate" name="taskStartDate" class="form-control" placeholder="Select Start Date" type="text" ng-model="task.taskStartDate"  />
                                </div>
                            </div>
                        </div>
                        <script type="text/javascript">
                            $(function () {
                                $('#taskStartDateField').datetimepicker({
                                    format: 'MM/DD/YYYY',
                                    viewMode: 'days',
                                    showTodayButton: true
                                });
                            });
                        </script>
                        <!-- Task End Date-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Task End Date</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group date" id="taskEndDateField">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                    <input id="taskEndDate" name="taskEndDate" class="form-control" placeholder="Select End Date" type="text" ng-model="task.taskEndDate"  />
                                </div>
                            </div>
                        </div>
                        <script type="text/javascript">
                            $(function () {
                                $('#taskEndDateField').datetimepicker({
                                    useCurrent: false,
                                    format: 'MM/DD/YYYY',
                                    viewMode: 'days',
                                    showTodayButton: true
                                });

                                $("#taskStartDateField").on("dp.change", function (e) {
                                    $('#taskEndDateField').data("DateTimePicker").minDate(e.date);
                                    $('#taskForm').formValidation('revalidateField', 'taskStartDate');
                                });
                                $("#taskEndDateField").on("dp.change", function (e) {
                                    $('#taskStartDateField').data("DateTimePicker").maxDate(e.date);
                                    $('#taskForm').formValidation('revalidateField', 'taskEndDate');
                                });
                            });
                        </script>
                        <!-- Assigned To-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Assigned To</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <select id="assignedTo" name="assignedTo" class="form-control" ng-model="task.assignedTo">
                                        <option value="">Select Associate</option>
                                        <option ng-repeat="tm in availableTeamMembers track by $index" value="{{tm.ntid}}">{{tm.name}}</option>
                                    </select>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button class="button" ng-click="taskHandler()">Submit</button>
                        <button class="button" data-dismiss="modal">Cancel</button>

                    </div>
                </form>
            </div>
        </div>
    </div>


    <!-- View Task Detail Modal -->
    <div class="modal fade" id="taskDetailModal" tabindex="-1" role="dialog"  aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">

                    <h4 style="text-align: center" class="modal-title" ">Task Detail</h4>
                </div>
               
                    <div class="modal-body">
                         <form id="taskDetailForm" class="form-horizontal">
                         <!--Task ID-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Task ID</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="detailTaskId" name="taskId" class="form-control" type="text" ng-model="currentTask.taskId" readonly="readonly" />
                                </div>
                            </div>
                        </div>

                        <!--Task Name-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Task Name</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="detailTaskName" name="taskName" class="form-control" type="text" ng-model="currentTask.taskName" readonly="readonly" />
                                </div>
                            </div>
                        </div>

                        <!-- Task Description-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Task Description</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <textarea id="detailTaskDesc" name="taskDesc" class="form-control" rows="2" cols="40" ng-model="currentTask.taskDesc" readonly="readonly"> </textarea>

                                </div>
                            </div>
                        </div>


                        <!-- Task Start Date-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Task Start Date</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                    <input id="detailTaskStartDate" name="taskStartDate" class="form-control" type="text" ng-model="currentTask.startDate" readonly="readonly" />
                                </div>
                            </div>
                        </div>

                        <!-- Task End Date-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Task End Date</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                    <input id="detailTaskEndDate" name="taskEndDate" class="form-control" type="text" ng-model="currentTask.expiryDate" readonly="readonly" />
                                </div>
                            </div>
                        </div>

                        <!-- Created On-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Created On</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="detailCreatedOn" type="text" name="createdOn" class="form-control" ng-model="currentTask.createdDate" readonly="readonly" />
                                </div>
                            </div>
                        </div>

                        <!-- Created By-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Created By</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="detailCreatedBy" type="text" name="createdBy" class="form-control" ng-model="currentTask.createdBy" readonly="readonly" />
                                </div>
                            </div>
                        </div>

                         <!-- Task Status-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Status</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="detailStatus" type="text" name="status" class="form-control" ng-model="currentTask.status" readonly="readonly" />
                                </div>
                            </div>
                        </div>
                        <!-- Assigned To-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Assigned To</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="detailAssignedTo" type="text" name="assignedTo" class="form-control" ng-model="currentTask.assignedTo" readonly="readonly" />
                                </div>
                            </div>
                        </div>
                              </form>
                    </div>
                    <div class="modal-footer">
                        <button class="button" data-dismiss="modal">Close</button>

                    </div>
          
            </div>
        </div>
    </div>

    <!-- Apply Leave Modal -->
    <div class="modal fade" id="leaveModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">

                    <h4 style="text-align: center" class="modal-title" ng-bind="leaveModalTitle"></h4>
                </div>
                <form id="leaveForm" class="form-horizontal">
                    <div class="modal-body">

                        <!-- Leave Start Date-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Leave Start Date</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group date" id="leaveStartDateField">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                    <input id="leaveStartDate" name="leaveStartDate" class="form-control" placeholder="Select Start Date" type="text" ng-model="leave.leaveStartDate"  />
                                </div>
                            </div>
                        </div>
                        <script type="text/javascript">
                            $(function () {
                                $('#leaveStartDateField').datetimepicker({
                                    format: 'MM/DD/YYYY',
                                    viewMode: 'days',
                                    showTodayButton: true
                                });
                            });
                        </script>
                        <!-- Leave End Date-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Leave End Date</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group date" id="leaveEndDateField">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                    <input id="leaveEndDate" name="leaveEndDate" class="form-control" placeholder="Select End Date" type="text" ng-model="leave.leaveEndDate"  />
                                </div>
                            </div>
                        </div>
                        <script type="text/javascript">
                            $(function () {
                                $('#leaveEndDateField').datetimepicker({
                                    useCurrent: false,
                                    format: 'MM/DD/YYYY',
                                    viewMode: 'days',
                                    showTodayButton: true
                                });

                                $("#leaveStartDateField").on("dp.change", function (e) {
                                    $('#leaveEndDateField').data("DateTimePicker").minDate(e.date);
                                    $('#leaveForm').formValidation('revalidateField', 'leaveStartDate');
                                });
                                $("#leaveEndDateField").on("dp.change", function (e) {
                                    $('#leaveStartDateField').data("DateTimePicker").maxDate(e.date);
                                    $('#leaveForm').formValidation('revalidateField', 'leaveEndDate');
                                });
                            });
                        </script>

                         <!-- Leave Type -->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Leave Type</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <select id="leaveType" name="leaveType" class="form-control" ng-model="leave.leaveType">
                                        <option value="">Select Leave Type</option>
                                        <option ng-repeat="lv in leaveTypes track by $index" value="{{lv}}">{{lv}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <!-- Leave Description-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Leave Description</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <textarea id="leaveDesc" name="leaveDesc" rows="2" cols="40" placeholder="Enter Detailed Reason" class="form-control" ng-model="leave.leaveDesc"> </textarea>

                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button class="button" ng-click="applyLeave()">Apply</button>
                        <button class="button" data-dismiss="modal">Cancel</button>

                    </div>
                </form>
            </div>
        </div>
    </div>

    <!-- View Leave Detail Modal -->
    <div class="modal fade" id="leaveDetailModal" tabindex="-1" role="dialog"  aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">

                    <h4 style="text-align: center" class="modal-title" ">Leave Detail</h4>
                </div>
               
                    <div class="modal-body">
                         <form id="leaveDetailForm" class="form-horizontal">
                         <!--Leave ID-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Leave ID</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="detailLeaveId" name="leaveId" class="form-control" type="text" ng-model="currentLeave.leaveId" readonly="readonly" />
                                </div>
                            </div>
                        </div>

                       

                        <!-- Leave Description-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Leave Description</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <textarea id="detailLeaveDesc" name="leaveDesc" class="form-control" rows="2" cols="40" ng-model="currentLeave.leaveDesc" readonly="readonly"> </textarea>

                                </div>
                            </div>
                        </div>

                        <!-- Leave Type -->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Leave Type</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="detailLeaveType" type="text" name="leaveType" class="form-control" ng-model="currentLeave.leaveType" readonly="readonly" />
                                </div>
                            </div>
                        </div>

                        <!-- Leave Start Date-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Leave Start Date</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                    <input id="detailLeaveFromDate" name="leaveFromDate" class="form-control" type="text" ng-model="currentLeave.fromDate" readonly="readonly" />
                                </div>
                            </div>
                        </div>

                        <!-- Leave End Date-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Leave End Date</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
                                    <input id="detailLeaveToDate" name="leaveToDate" class="form-control" type="text" ng-model="currentLeave.toDate" readonly="readonly" />
                                </div>
                            </div>
                        </div>

                       
                        <!-- Applied By-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Applied By</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="detailAppliedBy" type="text" name="appliedBy" class="form-control" ng-model="currentLeave.appliedBy" readonly="readonly" />
                                </div>
                            </div>
                        </div>

                         <!-- Leave Status-->
                        <div class="form-group">
                            <label class="col-md-4 control-label">Status</label>
                            <div class="col-md-6 inputGroupContainer">
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="glyphicon glyphicon-eye-open"></i></span>
                                    <input id="ldetailStatus" type="text" name="status" class="form-control" ng-model="currentLeave.status" readonly="readonly" />
                                </div>
                            </div>
                        </div>
                       
                     </form>
                    </div>
                    <div class="modal-footer">
                        <button class="button" data-dismiss="modal">Close</button>

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
                    <h4 class="modal-title" id="statusModalTitle" style="text-align: center" >Notification</h4>
                </div>
                <div class="modal-body">
                    <h3 style="text-align: center" ng-bind="reason"></h3>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">OK</button>
                </div>
            </div>
        </div>
    </div

 <!-- Loader -->
    <div style="position: fixed; top: 0; left: 0; right: 0; bottom: 0; margin: auto; z-index: 3500 ; display: none" class="loader la-ball-fussion la-3x">
        <div style="color: #d8edee;"></div>
        <div style="color: #ff414d;"></div>
        <div style="color: #1aa6b7;"></div>
        <div style="color: #022c42;"></div>
    </div>
    <div class="backdrop" style="z-index: 2500; display: none">
    </div>
</body>
</html>

