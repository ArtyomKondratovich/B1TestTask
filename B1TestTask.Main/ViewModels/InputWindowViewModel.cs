using B1TestTask.Main.ViewModels.Base;

namespace B1TestTask.Main.ViewModels
{
    internal class InputWindowViewModel : ViewModelBase
    {
        #region OkButtonContentProperty

        private string _okButtonContent = "Ок";
        public string OkButtunContent 
        {
            get => _okButtonContent;
            set => Set(ref _okButtonContent, value);
        }

        #endregion

        #region CancelButtonContentProperty

        private string _cancelButtonContent = "Отмена";
        public string CancelButtonContent
        {
            get => _cancelButtonContent;
            set => Set(ref _cancelButtonContent, value);
        }

        #endregion

        #region TextBlockProperty

        private string _textBlockText = "Ввод:";
        public string TextBlockText
        {
            get => _textBlockText;
            set => Set(ref _textBlockText, value);
        }

        #endregion

        #region TextBoxProperty

        private string _textBoxText = "";
        public string TextBoxText
        {
            get => _textBoxText;
            set => Set(ref _textBoxText, value);
        }

        #endregion

        public InputWindowViewModel() : base() { }
    }
}
