import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { UserListComponent } from './users/user-list/user-list.component';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { BatchListComponent } from './batches/batch-list/batch-list.component';
import { BatchEditComponent } from './batches/batch-edit/batch-edit.component';
import { LoginComponent } from './login/login.component';
import { RolesEditComponent } from './users/roles-edit/roles-edit.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { AuthGuard } from './_guards/auth.guard';
import { CheckedlistComponent } from './tasktypes/checkedlist/checkedlist.component';
import { RangeselectComponent } from './tasktypes/rangeselect/rangeselect.component';
import { RawmaterialsComponent } from './tasktypes/rawmaterials/rawmaterials.component';
import { UserTasksComponent } from './user-tasks/user-tasks.component';
import { ManagementComponent } from './management/management.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { AdminGuard } from './_guards/admin.guard';
import { EnterTaskGuard } from './_guards/enter-task.guard';
import { MessagesComponent } from './messages/messages.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { BatchTaskSummaryComponent } from './batch-task-summary/batch-task-summary.component';
import { BarcodeStickersComponent } from './barcode-stickers/barcode-stickers.component';
import { BatchRecordsReportComponent } from './batch-records-report/batch-records-report.component';




 
 

const routes: Routes = 
[
  {path:'home',component:HomeComponent},
  {path:'login',component:LoginComponent},
  {path:'users',component:UserListComponent,canActivate:[AdminGuard]},
  {path:'users/:id',component:UserEditComponent,canActivate:[AdminGuard]},
  {path:'userRegister',component:UserEditComponent,canActivate:[AdminGuard]},
  {path:'batches',component:BatchListComponent,canActivate:[AdminGuard]},
  {path:'batches/:id',component:BatchEditComponent,canActivate:[AdminGuard]},
  {path:'batchRegister',component:BatchEditComponent,canActivate:[AdminGuard]},
  {path:'userRoles/:userName',component:RolesEditComponent,canActivate:[AdminGuard]},
  {path:'notifications',component:NotificationsComponent,canActivate:[AuthGuard]},
  {path:'rawMaterial/:id',component:RawmaterialsComponent,canActivate:[EnterTaskGuard]},
  {path:'rangeSelect/:id',component:RangeselectComponent,canActivate:[EnterTaskGuard]},
  {path:'checkedList/:id',component:CheckedlistComponent,canActivate:[EnterTaskGuard]},
  {path:'userTasks',component:UserTasksComponent,canActivate:[AuthGuard]},
  {path:'management',component:ManagementComponent,canActivate:[AdminGuard]},
  {path:'not-found',component:NotFoundComponent},
  {path:'server-error',component:ServerErrorComponent},
  {path:'messages',component:MessagesComponent,canActivate:[AuthGuard]},
  {path:'changePassword/:id',component:ChangePasswordComponent,canActivate:[AdminGuard]},
  {path:'batchSummary/:id',component:BatchTaskSummaryComponent,canActivate:[AuthGuard]},
  {path:'batchReport/:id',component:BatchRecordsReportComponent,canActivate:[AuthGuard]},
  {path:'stickers',component:BarcodeStickersComponent,canActivate:[AuthGuard]},
  {path:'**',component:HomeComponent,pathMatch:'full'}
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
