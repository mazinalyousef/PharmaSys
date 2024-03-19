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
import {MatGridListModule} from '@angular/material/grid-list'
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
import { UserTasksComponent } from './user-tasks/user-tasks.component';
import { UserRunningTaskComponent } from './user-running-task/user-running-task.component';
import {MatCardModule} from '@angular/material/card';
import { IngredientListComponent } from './ingredients/ingredient-list/ingredient-list.component';
import { IngredientEditComponent } from './ingredients/ingredient-edit/ingredient-edit.component';
import { IngredientContainerComponent } from './ingredients/ingredient-container/ingredient-container.component';
import { ManagementComponent } from './management/management.component';
import { BarcodeContainerComponent } from './Barcodes/barcode-container/barcode-container.component';
import { BarcodeEditComponent } from './Barcodes/barcode-edit/barcode-edit.component';
import { BarcodeListComponent } from './Barcodes/barcode-list/barcode-list.component';
import { ProductContainerComponent } from './Products/product-container/product-container.component';
import { ProductEditComponent } from './Products/product-edit/product-edit.component';
import { ProductListComponent } from './Products/product-list/product-list.component';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import {MatPaginatorModule} from '@angular/material/paginator';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component'
import { ToastrModule } from 'ngx-toastr';
import { MessagesComponent } from './messages/messages.component';
import { MessageComponent } from './message/message.component';  
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner'
import { BusyInterceptor } from './_interceptors/busy.interceptor';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { NgxSpinnerModule } from 'ngx-spinner';

 





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
    UserTasksComponent,
    UserRunningTaskComponent,
    IngredientListComponent,
    IngredientEditComponent,
    IngredientContainerComponent,
    ManagementComponent,
    BarcodeContainerComponent,
    BarcodeEditComponent,
    BarcodeListComponent,
    ProductContainerComponent,
    ProductEditComponent,
    ProductListComponent,
    TestErrorsComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MessagesComponent,
    MessageComponent,
    ChangePasswordComponent
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
    MatAutocompleteModule,
    MatBadgeModule,
    MatDialogModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCheckboxModule,
    RouterModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatCardModule,
    MatSnackBarModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatGridListModule,
    NgxSpinnerModule,
    
    ToastrModule.forRoot(
      {positionClass: 'toast-bottom-right'}
    ),
    RouterModule.forRoot([])
  ],
  providers: [
    {provide:HTTP_INTERCEPTORS,useClass:JwtInterceptor,multi:true},
    {provide:HTTP_INTERCEPTORS,useClass:ErrorInterceptor,multi:true},
    {provide:HTTP_INTERCEPTORS,useClass:BusyInterceptor,multi:true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
