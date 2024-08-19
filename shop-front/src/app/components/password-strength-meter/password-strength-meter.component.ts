import {
  AfterContentChecked,
  AfterViewChecked,
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';

@Component({
  selector: 'app-password-strength-meter',
  standalone: true,
  imports: [],
  templateUrl: './password-strength-meter.component.html',
  styleUrl: './password-strength-meter.component.css',
})
export class PasswordStrengthMeterComponent implements OnChanges {
  @Input() max!: number;
  array = Array(this.max).fill(false);
  @Input() current!: number;
  @Input() error!: string;
  ngOnChanges(changes: SimpleChanges): void {
    this.array = Array(this.max)
      .fill(false)
      .map((v, i) => this.current > i);
    // console.log(this.array);
  }
}
