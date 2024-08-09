import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Category } from '../../interfaces/category';
import { CategoryService } from '../../Services/category.service';
import { AddnewcategoryComponent } from '../modal/addnewcategory/addnewcategory.component';
import { ModalNavigateService } from '../../Services/modal-navigate.service';
import { EditCategoryComponent } from '../modal/edit-category/edit-category.component';

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

  constructor(
    private categoryService: CategoryService,
    private cdr: ChangeDetectorRef,
    private modal: ModalNavigateService
  ) {}

  ngOnInit() {
    this.getAllCategories();
    this.categoryService.changeCateogryPrices.subscribe(() => {
      this.getAllCategories();
    });
  }

  addNewCategory() {
    console.log('Routing');

    this.modal.goToModal([AddnewcategoryComponent.Path]);
  }

  async getAllCategories() {
    const response = await this.categoryService.getAllCategories();
    if (response.ok) {
      this.categories =
        response.body?.map<Category>((c) => {
          return {
            Id: c.id,
            Name: c.name,
            Standard: c.standard,
            Price: c.price,
            IsPrimary: c.isPrimary,
          };
        }) ?? [];

      // Repeat the list 10 times
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
