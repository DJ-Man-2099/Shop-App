import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { AddnewcategoryComponent } from './components/modal/addnewcategory/addnewcategory.component';
import { EditCategoryComponent } from './components/modal/edit-category/edit-category.component';
import { MessageComponent } from './components/modal/message/message.component';

export const routes: Routes = [
  { path: '', component: AppComponent },
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
];
