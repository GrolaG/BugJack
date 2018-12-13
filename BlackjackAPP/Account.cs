namespace BlackjackAPP
{
    public class Account
    {
        public static int step = 10; //шаг изменения счёта используется только извне!
        public int Value { get; set; } //значение счёта в ставках

        public Account()
        {
            this.Value = 10;
        }
        public void Deposit(int bids)// взять деньги, метод принимает сумму для зачисления
        {
            Value += bids;
        }
        public bool Withdraw(int bids) // взять деньги, метод принимает количество шагов и результат списания
        {
            if (Value >= bids)
            {
                Value -= bids;
                return true;
            }
            else
            {
                return false;
            }
        }
    }


}
