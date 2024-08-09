import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { AddnewcategoryComponent } from './components/modal/addnewcategory/addnewcategory.component';
import { EditCategoryComponent } from './components/modal/edit-category/edit-category.component';
import { MessageComponent } from './components/modal/message/message.component';
import { CategoriesListComponent } from './components/categories-list/categories-list.component';
import { SideBarComponent } from './components/modal/side-bar/side-bar.component';

export const routes: Routes = [
  { path: CategoriesListComponent.Path, component: CategoriesListComponent },
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
    path: SideBarComponent.Path,
    component: SideBarComponent,
    outlet: 'modal',
  },
];
