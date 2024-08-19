import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Category } from '../../interfaces/category';
import { CategoryService } from '../../Services/category.service';
import { AddnewcategoryComponent } from '../modal/addnewcategory/addnewcategory.component';
import { ModalNavigateService } from '../../Services/modal-navigate.service';
import { EditCategoryComponent } from '../modal/edit-category/edit-category.component';
import { AuthenticationService } from '../../Services/authentication.service';

@Component({
  selector: 'app-categories-list',
  standalone: true,
  imports: [],
  templateUrl: './categories-list.component.html',
  styleUrl: './categories-list.component.css',
})
export class CategoriesListComponent implements OnInit {
  static Path = 'categories-list';

  categories: Category[] = [];
  isEdible = false;

  constructor(
    private categoryService: CategoryService,
    private cdr: ChangeDetectorRef,
    private modal: ModalNavigateService,
    private authService: AuthenticationService
  ) {
    this.isEdible = this.authService.user?.role === 'Admin';
  }

  ngOnInit() {
    this.getAllCategories();
    this.categoryService.changeCateogryPrices.subscribe(() => {
      this.getAllCategories();
    });
    this.categoryService.changeBaseCategoryEvent.subscribe(() => {
      this.getAllCategories();
    });
  }

  addNewCategory() {
    this.modal.goToModal([AddnewcategoryComponent.Path]);
  }

  async getAllCategories() {
    const response = await this.categoryService.getAllCategories();
    if (response.ok) {
      const categories =
        response.body?.map<Category>((c) => {
          return {
            Id: c.id,
            Name: c.name,
            Standard: c.standard,
            Price: c.price,
            Type: c.type,
          };
        }) ?? [];

      // Repeat the list 10 times
      this.categories = categories;
      // .reduce((acc, cur) => {
      //   for (let i = 0; i < 10; i++) {
      //     acc.push(cur);
      //   }
      //   return acc;
      // }, [] as Category[])
      this.cdr.detectChanges();
    }
  }

  editCategory(category: Category) {
    this.modal.goToModal(
      [EditCategoryComponent.Path, category.Id!.toString()],
      category
    );
  }
}
