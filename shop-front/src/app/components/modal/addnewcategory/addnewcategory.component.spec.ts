import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddnewcategoryComponent } from './addnewcategory.component';

describe('AddnewcategoryComponent', () => {
  let component: AddnewcategoryComponent;
  let fixture: ComponentFixture<AddnewcategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddnewcategoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddnewcategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
