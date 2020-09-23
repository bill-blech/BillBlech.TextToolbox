using BillBlech.TextToolbox.Activities.Activities.Encryption;
using BillBlech.TextToolbox.Activities.Properties;
using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace BillBlech.TextToolbox.Activities
{
    [LocalizedDisplayName(nameof(Resources.EncryptValue_DisplayName))]
    [LocalizedDescription(nameof(Resources.EncryptValue_Description))]
    public class EncryptValue : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.EncryptValue_PlainText_DisplayName))]
        [LocalizedDescription(nameof(Resources.EncryptValue_PlainText_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> PlainText { get; set; }

        [LocalizedDisplayName(nameof(Resources.EncryptValue_EncryptedText_DisplayName))]
        [LocalizedDescription(nameof(Resources.EncryptValue_EncryptedText_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> EncryptedText { get; set; }

        [LocalizedDisplayName(nameof(Resources.EncryptValue_EncryptionAlgorithm_DisplayName))]
        [LocalizedDescription(nameof(Resources.EncryptValue_EncryptionAlgorithm_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public enumEncryptType EncryptionAlgorithm { get; set; }

        public enum enumEncryptType
        {
            SHA256,
            AES,
            HMAC,
            ASCIII
        }



        #endregion


        #region Constructors

        public EncryptValue()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (PlainText == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(PlainText)));
            if (EncryptedText == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(EncryptedText)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var encryptionAlgorithm = EncryptionAlgorithm;
            var plainText = PlainText.Get(context);

            string encryptedText = null;

            ///////////////////////////
            // Add execution logic HERE
            string password = "BillBlechBillBlechBillBlechBillBlechBillBlech";

            //Select algorithm
            switch (EncryptionAlgorithm)
            {
                //SHA256
                case enumEncryptType.SHA256:
                    encryptedText = Cipher.Encrypt(plainText, password);
                    break;
                //AES
                case enumEncryptType.AES:
                    encryptedText = AES.EncryptStringAES(plainText, password);
                    break;
                //HMAC
                case enumEncryptType.HMAC:
                    encryptedText = HMAC.SimpleEncryptWithPassword(plainText, password);
                    break;
                //ASCIII
                case enumEncryptType.ASCIII:
                    encryptedText = ASCIII.Encrypt(plainText);
                    break;
            }
            ///////////////////////////

            // Outputs
            return (ctx) =>
            {
                EncryptedText.Set(ctx, encryptedText);
            };
        }

        #endregion
    }
}

