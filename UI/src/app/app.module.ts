import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import{MatButtonModule} from '@angular/material/button';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatMenuModule} from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import {MatTableModule} from '@angular/material/table';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { NavComponent } from './nav/nav.component';
import { RouterModule, Routes }   from '@angular/router';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatTooltipModule } from '@angular/material/tooltip';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { ReactiveFormsModule } from '@angular/forms';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import {MatTabsModule} from '@angular/material/tabs';
import {MatDialogModule} from '@angular/material/dialog'
import {MatBadgeModule} from '@angular/material/badge';
import{MatCheckboxModule} from '@angular/material/checkbox'
import { UserListComponent } from './users/user-list/user-list.component';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { BatchEditComponent } from './batches/batch-edit/batch-edit.component';
import { BatchListComponent } from './batches/batch-list/batch-list.component';
import { HomeComponent } from './home/home.component';
import { DataService } from './_services/data.service';
import { DepartmentsService } from './_services/departments.service';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { LoginComponent } from './login/login.component';
import { RolesEditComponent } from './users/roles-edit/roles-edit.component';
import { HasRoleDirective } from './_directives/has-role.directive';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { NotificationsComponent } from './notifications/notifications.component';
import { CheckedlistComponent } from './tasktypes/checkedlist/checkedlist.component';
import { RawmaterialsComponent } from './tasktypes/rawmaterials/rawmaterials.component';
import { RangeselectComponent } from './tasktypes/rangeselect/rangeselect.component';




@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    UserListComponent,
    UserEditComponent,
    BatchEditComponent,
    BatchListComponent,
    HomeComponent,
    ConfirmDialogComponent,
    LoginComponent,
    RolesEditComponent,
    HasRoleDirective,
    NotificationsComponent,
    CheckedlistComponent,
    RawmaterialsComponent,
    RangeselectComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FormsModule,
    MatButtonModule,
    MatSidenavModule,
    MatMenuModule,
    MatToolbarModule,
    MatTabsModule,
    MatIconModule,
    MatListModule,
    MatTableModule,
    MatExpansionModule,
    MatTooltipModule,
    MatFormFieldModule,
    MatInputModule,
    MatBadgeModule,
    MatDialogModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCheckboxModule,
    RouterModule,
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule.forRoot([])
  ],
  providers: [
    {provide:HTTP_INTERCEPTORS,useClass:JwtInterceptor,multi:true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
