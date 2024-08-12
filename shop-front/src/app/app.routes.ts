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

// function isBrowser(): boolean {
//   return isPlatformBrowser(inject(PLATFORM_ID));
// }

export const routes: Routes = [
  {
    path: LoginComponent.Path,
    component: LoginComponent,
  },
  {
    path: MainPageComponent.Path,
    component: MainPageComponent,
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
        path: `${SignUpComponent.Path}/:role`,
        component: SignUpComponent,
      },
      {
        path: AddnewcategoryComponent.Path,
        component: AddnewcategoryComponent,
        outlet: 'modal',
      },
      {
        path: `${EditCategoryComponent.Path}/:id`,
        component: EditCategoryComponent,
        outlet: 'modal',
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
      },
      {
        path: `${EditGroupComponent.Path}/:id`,
        component: EditGroupComponent,
        outlet: 'modal',
      },
      {
        path: AddNewProductComponent.Path,
        component: AddNewProductComponent,
        outlet: 'modal',
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
      },
      {
        path: SideBarComponent.Path,
        component: SideBarComponent,
        outlet: 'modal',
      },
    ],
  },
  // {
  //   path: '**',
  //   redirectTo: '',
  // },
];
