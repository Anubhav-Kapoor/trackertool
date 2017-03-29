<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Task_and_Leave_Tracker.Index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <link rel="import" href="header.html">
    <title>Task & Leave Tracker Tool</title>
</head>
<body ng-app="myApp" ng-controller="loginCtrl" style="background-image :url(images/background_pattern.jpg);" ng-init="refreshTable()" >
    
    <div class="container type-wrap" style="height:auto">
        
        <div header></div>
        <!--Form section-->
        <div class="row">
            <div class="col-6">
                <i class="fa fa-plus-circle"></i><button type="button" class="button">Create Task</button>

            </div>
            <div class="col-6">
                <p style="margin-top:20px">{{time}}</p>

            </div>
        </div>

        <!-- Data Table -->
        <div class="row" style="margin-top:20px">
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
        <div class="row" style="margin-top:20px">

            <div class="col-6">
                <!--email sending-->
                <button type="button" class="button"  >Email Pending Tasks</button>
            </div>
            <div class="col-6">
                
                <!--Mark All completion-->
                <button type="button" class="button" style="float: right;">Mark All Completed</button>
            </div>
        </div>

</div>
</body>
</html>

