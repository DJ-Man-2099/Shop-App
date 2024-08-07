import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { AddnewcategoryComponent } from './components/modal/addnewcategory/addnewcategory.component';
import { EditCategoryComponent } from './components/modal/edit-category/edit-category.component';

export const routes: Routes = [
  { path: '', component: AppComponent },
  {
    path: AddnewcategoryComponent.Path,
    component: AddnewcategoryComponent,
    outlet: 'modal',
  },
  {
    path: `${EditCategoryComponent.Path}/:standard`,
    component: EditCategoryComponent,
    outlet: 'modal',
  },
];
