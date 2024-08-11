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
        path: AddNewProductComponent.Path,
        component: AddNewProductComponent,
        outlet: 'modal',
      },
      {
        path: SideBarComponent.Path,
        component: SideBarComponent,
        outlet: 'modal',
      },
    ],
  },
];
