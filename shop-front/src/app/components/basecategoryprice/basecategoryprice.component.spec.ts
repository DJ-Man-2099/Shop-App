import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasecategorypriceComponent } from './basecategoryprice.component';

describe('BasecategorypriceComponent', () => {
  let component: BasecategorypriceComponent;
  let fixture: ComponentFixture<BasecategorypriceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BasecategorypriceComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BasecategorypriceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
