import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowProductInfoComponent } from './show-product-info.component';

describe('ShowProductInfoComponent', () => {
  let component: ShowProductInfoComponent;
  let fixture: ComponentFixture<ShowProductInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShowProductInfoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShowProductInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
