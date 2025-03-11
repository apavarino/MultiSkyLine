using System;

namespace MSL.model
{
    /**
     * Represents a contract between two players for resource exchange.
     *
     * A contract can have two states:
     * - **Inactive**: The offering player has proposed a contract, waiting for another player to accept.
     * - **Active**: The contract has been accepted, and an exchange is ongoing between two players.
     *
     * This contract allows trading of resources such as electricity and water, specifying the amount, price, and efficiency of the exchange.
     */
    [Serializable]
    public class Contract
    {
        /** The player who initiated the contract. */
        public string From { get; set; }
        
        /** The player who accepted the contract. */
        public string To { get; set; }
        
        /** The type of resource being exchanged */
        public ContractType Type { get; set; }
        
        /** The quantity of the resource being exchanged. */
        public int Amount { get; set; }
        
        /** The price agreed upon for the exchanged resource. */
        public int Price { get; set; }
        
        /** Indicates whether the contract is active (true) or pending acceptance (false). */
        public bool Active { get; set; }
        /**
        * Represents the efficiency percentage of the exchange.
        *
        * If the offering player is unable to fully meet the agreed-upon amount,dz
        * the efficiency is reduced accordingly. A lower efficiency impacts the exchange
        * by decreasing the effective delivery of resources and adjusting the price proportionally.
        */
        public int Efficiency { get; set; }
    }

    /**
     * Defines the type of resource involved in the contract.
     */
    public enum ContractType
    {
        Electric,
        Water
    }
}