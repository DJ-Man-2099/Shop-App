import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CategoryService } from '../../Services/category.service';
import { BaseCategoryInfo } from '../../interfaces/category';

@Component({
  selector: 'app-basecategoryprice',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './basecategoryprice.component.html',
  styleUrl: './basecategoryprice.component.css',
})
export class BasecategorypriceComponent implements OnInit {
  baseCategory!: BaseCategoryInfo;
  baseCategoryPrice!: number;

  constructor(
    private categoryService: CategoryService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.categoryService.getBaseCategory().subscribe({
      next: (response) => {
        if (response.body) {
          this.baseCategory = response.body as {
            standard: number;
            price: number;
          };
          this.baseCategoryPrice = this.baseCategory.price ?? 0;
          this.cdr.detectChanges();
        } else {
          console.error('Response body is null');
        }
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  resetBaseCategoryPrice(): void {
    this.baseCategoryPrice = this.baseCategory?.price ?? 0;
  }

  async changeBaseCategoryPrice(): Promise<void> {
    const response = await this.categoryService.changeBaseCategoryPrice(
      this.baseCategoryPrice
    );
    console.log(response.body);
    if (response.ok) {
      this.baseCategory = {
        standard: response.body!.standard,
        price: response.body!.price,
      };
    } else {
      console.error('Failed to change base category price');
    }
  }
}
