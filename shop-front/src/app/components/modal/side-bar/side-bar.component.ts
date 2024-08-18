import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ModalNavigateService } from '../../../Services/modal-navigate.service';
import { CategoriesListComponent } from '../../categories-list/categories-list.component';
import { AppComponent } from '../../../app.component';
import { Router, RouterModule } from '@angular/router';
import { AuthenticationService } from '../../../Services/authentication.service';
import { GroupsListComponent } from '../../groups-list/groups-list.component';
import { ProductsListComponent } from '../../products-list/products-list.component';
import { SignUpComponent } from '../../sign-up/sign-up.component';
import { MainPageComponent } from '../../main-page/main-page.component';
import { LoginComponent } from '../../login/login.component';
import { MessageModalService } from '../../../Services/message-modal.service';

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
    { name: 'الصفحة الرئيسية', path: MainPageComponent.Path },
    { name: 'العيارات', path: CategoriesListComponent.Path },
    { name: 'المجموعات', path: GroupsListComponent.Path },
    { name: 'المنتجات', path: ProductsListComponent.Path },
  ];

  constructor(
    private modal: ModalNavigateService,
    private messageModal: MessageModalService,
    private router: Router,
    private authService: AuthenticationService
  ) {
    this.name = this.authService.user?.name ?? '';
    this.role = this.authService.user?.role ?? '';
    if (this.role === 'Admin') {
      this.links.push({
        name: 'تسجيل مستخدم جديد',
        path: SignUpComponent.Path,
      });
    }
  }

  dismiss() {
    this.modal.dismiss();
  }

  name!: string;
  role!: string;

  async logout() {
    this.dismiss();
    const res = await this.messageModal.showSuccessMessage(
      'تم تسجيل الخروج بنجاح',
      'تسجيل الخروج'
    );
    console.log(`message from modal: ${res}`);

    // await this.authService.clearToken();
    // this.router.navigate([
    //   { outlets: { primary: [LoginComponent.Path], modal: null } },
    // ]);
  }

  goto(path: string | string[]) {
    this.dismiss();
    this.router.navigate([
      { outlets: { primary: path === '' ? null : path, modal: null } },
    ]);
  }
}
