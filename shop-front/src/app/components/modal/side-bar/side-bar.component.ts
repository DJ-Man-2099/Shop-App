import { Component } from '@angular/core';
import { ModalNavigateService } from '../../../Services/modal-navigate.service';
import { CategoriesListComponent } from '../../categories-list/categories-list.component';
import { AppComponent } from '../../../app.component';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-side-bar',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.css',
})
export class SideBarComponent {
  static Path = 'side-bar';

  links = [
    { name: 'الصفحة الرئيسية', path: '' },
    { name: 'العيارات', path: CategoriesListComponent.Path },
  ];

  constructor(private modal: ModalNavigateService, private router: Router) {}

  dismiss() {
    this.modal.dismiss();
  }

  goto(path: string) {
    this.modal.dismiss();
    if (path === '') {
      this.router.navigate(['']);
    } else {
      this.router.navigate([{ outlets: { primary: [path], modal: null } }], {
        replaceUrl: true,
      });
    }
  }
}
