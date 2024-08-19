import { Component, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import {
  EnhancedFormBuilderService,
  FormKeys,
} from '../../../Services/enhanced-form-builder.service';
import { ModalNavigateService } from '../../../Services/modal-navigate.service';
import { GroupService } from '../../../Services/group.service';
import { CategoryService } from '../../../Services/category.service';
import { LoadingComponent } from '../../loading/loading.component';
import { MessageModalService } from '../../../Services/message-modal.service';

@Component({
  selector: 'app-add-new-group',
  standalone: true,
  imports: [ReactiveFormsModule, LoadingComponent],
  templateUrl: './add-new-group.component.html',
  styleUrl: './add-new-group.component.css',
})
export class AddNewGroupComponent implements OnInit {
  static Path = 'add-new-group';

  isLoading = true;

  newGroupForm!: FormGroup;
  newGroupFormControlNames!: FormKeys[];

  constructor(
    private efb: EnhancedFormBuilderService,
    private modal: ModalNavigateService,
    private groupService: GroupService,
    private categoryService: CategoryService,
    private messageService: MessageModalService
  ) {}

  async ngOnInit() {
    var categories = await this.categoryService.getAllCategories();
    if (!categories.ok) {
      console.log('error');
    }
    this.isLoading = false;
    var options = categories.body?.map((c) => ({
      label: c.name,
      value: c.id,
    }));
    this.efb.createForm({
      name: {
        type: 'text',
        displayName: 'أسم المجموعة',
        controls: ['', Validators.required],
      },
      categoryId: {
        type: 'select',
        displayName: 'العيار',
        options: options,
        controls: [options![0].value, [Validators.required, Validators.min(0)]],
      },
    });

    this.newGroupForm = this.efb.resultForm;
    this.newGroupFormControlNames = this.efb.getFormControlNames();
  }

  async onSubmit() {
    var res = await this.groupService.AddNewGroup(this.newGroupForm.value);
    if (res.ok) {
      this.groupService.changeGroups.emit();
      this.modal.dismiss();
    } else {
      await this.messageService.showErrorMessage(
        'المجموعة مضافة مسبقًا',
        'خطأ'
      );
    }
  }

  onDismiss() {
    this.modal.dismiss();
  }
}
