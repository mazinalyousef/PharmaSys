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


 
 

const routes: Routes = 
[
  {path:'home',component:HomeComponent},
  {path:'login',component:LoginComponent},
  {path:'users',component:UserListComponent,canActivate:[AuthGuard]},
  {path:'users/:id',component:UserEditComponent},
  {path:'userRegister',component:UserEditComponent},
  {path:'batches',component:BatchListComponent},
  {path:'batches/:id',component:BatchEditComponent},
  {path:'batchRegister',component:BatchEditComponent},
  {path:'userRoles/:userName',component:RolesEditComponent},
  {path:'notifications',component:NotificationsComponent},
  {path:'rawMaterial/:id',component:RawmaterialsComponent},
  {path:'rangeSelect/:id',component:RangeselectComponent},
  {path:'checkedList/:id',component:CheckedlistComponent},
  {path:'userTasks',component:UserTasksComponent},
  {path:'**',component:HomeComponent,pathMatch:'full'}
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
