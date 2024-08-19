import { Routes } from '@angular/router';
import { AddnewcategoryComponent } from './components/modal/addnewcategory/addnewcategory.component';
import { EditCategoryComponent } from './components/modal/edit-category/edit-category.component';
import { MessageComponent } from './components/modal/message/message.component';
import { CategoriesListComponent } from './components/categories-list/categories-list.component';
import { SideBarComponent } from './components/modal/side-bar/side-bar.component';
import { LoginComponent } from './components/login/login.component';
import { MainPageComponent } from './components/main-page/main-page.component';
import { AddNewGroupComponent } from './components/modal/add-new-group/add-new-group.component';
import { AddNewProductComponent } from './components/modal/add-new-product/add-new-product.component';
import { GroupsListComponent } from './components/groups-list/groups-list.component';
import { EditGroupComponent } from './components/modal/edit-group/edit-group.component';
import { ShowProductInfoComponent } from './components/modal/show-product-info/show-product-info.component';
import { ProductsListComponent } from './components/products-list/products-list.component';
import { EditProductComponent } from './components/modal/edit-product/edit-product.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { isPlatformBrowser } from '@angular/common';
import { inject, PLATFORM_ID } from '@angular/core';
import { AuthGuard } from './Guards/auth.guard';
import { adminGuard } from './Guards/admin.guard';

function isAuthenticated(): boolean {
  return isPlatformBrowser(inject(PLATFORM_ID));
}

export const routes: Routes = [
  {
    path: AddnewcategoryComponent.Path,
    component: AddnewcategoryComponent,
    outlet: 'modal',
    canActivate: [adminGuard],
  },
  {
    path: `${EditCategoryComponent.Path}/:id`,
    component: EditCategoryComponent,
    outlet: 'modal',
    canActivate: [adminGuard],
  },
  {
    path: MessageComponent.Path,
    component: MessageComponent,
    outlet: 'modal',
  },
  {
    path: AddNewGroupComponent.Path,
    component: AddNewGroupComponent,
    outlet: 'modal',
    canActivate: [adminGuard],
  },
  {
    path: `${EditGroupComponent.Path}/:id`,
    component: EditGroupComponent,
    outlet: 'modal',
    canActivate: [adminGuard],
  },
  {
    path: AddNewProductComponent.Path,
    component: AddNewProductComponent,
    outlet: 'modal',
    canActivate: [adminGuard],
  },
  {
    path: `${ShowProductInfoComponent.Path}/:id`,
    component: ShowProductInfoComponent,
    outlet: 'modal',
  },
  {
    path: `${EditProductComponent.Path}/:id`,
    component: EditProductComponent,
    outlet: 'modal',
    canActivate: [adminGuard],
  },
  {
    path: SideBarComponent.Path,
    component: SideBarComponent,
    outlet: 'modal',
  },
  {
    path: LoginComponent.Path,
    component: LoginComponent,
  },
  {
    path: MainPageComponent.Path,
    component: MainPageComponent,
    canActivate: [AuthGuard], // Apply the AuthGuard to the main page route
    children: [
      {
        path: CategoriesListComponent.Path,
        component: CategoriesListComponent,
      },
      {
        path: GroupsListComponent.Path,
        component: GroupsListComponent,
      },
      {
        path: ProductsListComponent.Path,
        component: ProductsListComponent,
      },
      {
        path: SignUpComponent.Path,
        component: SignUpComponent,
        canActivate: [adminGuard],
      },
    ],
  },
  // {
  //   path: '**',
  //   redirectTo: '',
  // },
];
