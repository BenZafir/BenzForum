import { Component, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import {
  ClassicEditor,
  Bold,
  Essentials,
  Heading,
  Indent,
  IndentBlock,
  Italic,
  Link,
  List,
  MediaEmbed,
  Paragraph,
  Table,
  Undo,
} from 'ckeditor5';
import 'ckeditor5/ckeditor5.css';

@Component({
  selector: 'app-rich-text',
  standalone: true,
  imports: [CKEditorModule, FormsModule],
  templateUrl: './rich-text.component.html',
  styleUrls: ['./rich-text.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RichTextComponent),
      multi: true,
    },
  ],
})
export class RichTextComponent implements ControlValueAccessor {
  @Input() value: string = '';
  public Editor = ClassicEditor;
  public config = {
    toolbar: [
      'undo',
      'redo',
      '|',
      'heading',
      '|',
      'bold',
      'italic',
      '|',
      'link',
      'insertTable',
      'mediaEmbed',
      '|',
      'bulletedList',
      'numberedList',
      'indent',
      'outdent',
    ],
    plugins: [
      Bold,
      Essentials,
      Heading,
      Indent,
      IndentBlock,
      Italic,
      Link,
      List,
      MediaEmbed,
      Paragraph,
      Table,
      Undo,
    ],
  };

  // ControlValueAccessor implementation
  private onChange: (value: string) => void = () => {};
  private onTouched: () => void = () => {};

  // Write a new value to the element
  writeValue(value: string): void {
    this.value = value;
  }

  // Notify when the value changes
  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  // Notify when the element is touched (blur event)
  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  // Handle CKEditor change events and propagate the value
  onEditorChange(value: string): void {
    this.value = value;
    this.onChange(value); // propagate the change to the outside form
  }
}
