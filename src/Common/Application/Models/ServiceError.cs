using System;

namespace Application.Models
{
    /// <summary>
    /// All errors contained in ServiceResult objects must return an error of this type
    /// Error codes allow the caller to easily identify the received error and take action.
    /// Error messages allow the caller to easily show error messages to the end user.
    /// </summary>
    [Serializable]
    public class ServiceError
    {
        /// <summary>
        /// CTOR
        /// </summary>
        public ServiceError(string message, int code) {
            Message = message;
            Code = code;
        }

        public ServiceError() { }

        /// <summary>
        /// Human readable error message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Machine readable error code
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// Default error for when we receive an exception
        /// </summary>
        public static ServiceError DefaultError => new ServiceError("Erro desconhecido. Entre em contato com o suporte.", 999);

        /// <summary>
        /// Default validation error. Use this for invalid parameters in controller actions and service methods.
        /// </summary>
        public static ServiceError ModelStateError(string validationError) {
            return new ServiceError(validationError, 998);
        }
        public static ServiceError SemAssinatura => new ServiceError("Não possui assinatura", 1014);
        public static ServiceError PromocaoJaAvaliada => new ServiceError("Promoção já avaliada", 1013);

        public static ServiceError PromocaoForaDoPrazoValidade => new ServiceError("A promoção fora do prazo de utilizacao", 1012);
        public static ServiceError InvalidRefreshToken => new ServiceError("O token é inválido.", 1011);
        public static ServiceError InvalidUserGuestRole => new ServiceError("O usuário não é um convidado.", 1010);
        public static ServiceError ExistUser => new ServiceError("Este usuário já está cadastrado no sistema.", 1009);
        public static ServiceError GuestUserExistInGuests => new ServiceError("Este número de telefone está registrado como convidado.", 1008);
        public static ServiceError UserIsNotAdmin => new ServiceError("Os usuários administradores só têm permissão para entrar no painel de controle.", 1007);
        public static ServiceError GuestUserExistInUsers => new ServiceError("Este número de telefone já esta registrado em uma conta.", 1006);
        public static ServiceError GuestUserInvalidDate => new ServiceError("O usuário convidado foi usado por mais de 20 dias e deve criar uma conta.", 1005);

        public static ServiceError DuplicatePhoneNumberOrEmail => new ServiceError("Existe um usuário com este perfil.", 1004);
        public static ServiceError InvalidVerifyCode => new ServiceError("O código de confirmação do número de telefone não é válido.", 1003);
        public static ServiceError EmptyVerifyCode => new ServiceError("O código de verificação é inválido.", 1002);
        public static ServiceError CreateUserException => new ServiceError("Erro ao registrar a conta.", 1000);

        public static ServiceError UserIsLockedOut => new ServiceError("Esta conta foi encerrada.", 1001);
        /// <summary>
        /// Use this for unauthorized responses.
        /// </summary>
        public static ServiceError ForbiddenError => new ServiceError("Você não tem permissão para acessar esse recurso.", 998);

        /// <summary>
        /// Use this to send a custom error message
        /// </summary>
        public static ServiceError CustomMessage(string errorMessage) {
            return new ServiceError(errorMessage, 999);

        }

        public static ServiceError InccrrectUsernameOrPassword => new ServiceError("Email ou senha inválida.", 997);
        public static ServiceError UserInactive => new ServiceError("Usuario Inativo.", 904);
        public static ServiceError UserNotFound => new ServiceError("Nenhum usuário encontrado.", 996);

        public static ServiceError UserFailedToCreate => new ServiceError("Erro ao registrar o usuário.", 995);
        public static ServiceError UserFailedToDelete => new ServiceError("Erro ao deletar o usuário.", 906);


        public static ServiceError NotFound => new ServiceError("The specified resource was not found.", 990);

        public static ServiceError ErrorInSaveOrUpdate => new ServiceError("Erro ao gravar informações.", 902);
        public static ServiceError ValidationFormat => new ServiceError("Request object format is not true.", 901);

        public static ServiceError Validation => new ServiceError("One or more validation errors occurred.", 900);

        public static ServiceError SearchAtLeastOneCharacter => new ServiceError("Search parameter must have at least one character!", 898);

        /// <summary>
        /// Default error for when we receive an exception
        /// </summary>
        public static ServiceError ServiceProviderNotFound => new ServiceError("Service Provider with this name does not exist.", 700);

        public static ServiceError ServiceProvider => new ServiceError("Service Provider failed to return as expected.", 600);

        public static ServiceError DateTimeFormatError => new ServiceError("Date format is not true. Date format must be like yyyy-MM-dd (2019-07-19)", 500);

        #region Override Equals Operator

        /// <summary>
        /// Use this to compare if two errors are equal
        /// Ref: https://msdn.microsoft.com/ru-ru/library/ms173147(v=vs.80).aspx
        /// </summary>
        public override bool Equals(object obj) {
            // If parameter cannot be cast to ServiceError or is null return false.
            var error = obj as ServiceError;

            // Return true if the error codes match. False if the object we're comparing to is nul
            // or if it has a different code.
            return Code == error?.Code;
        }

        public bool Equals(ServiceError error) {
            // Return true if the error codes match. False if the object we're comparing to is nul
            // or if it has a different code.
            return Code == error?.Code;
        }

        public override int GetHashCode() {
            return Code;
        }

        public static bool operator ==(ServiceError a, ServiceError b) {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b)) {
                return true;
            }

            // If one is null, but not both, return false.
            if ((object)a == null || (object)b == null) {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(ServiceError a, ServiceError b) {
            return !(a == b);
        }

        #endregion
    }

}
